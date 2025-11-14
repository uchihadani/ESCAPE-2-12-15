using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyPerception))]
[RequireComponent(typeof(EnemyAttack))]
public class EnemyAI : MonoBehaviour
{
    private enum State { Patrol, Chase, Attack }

    [Header("Objetivo")]
    [SerializeField] private Transform player; // si lo dejás vacío, lo busco por Tag "Player" en Start

    [Header("Patrulla")]
    [SerializeField] private List<Transform> patrolPoints = new();
    [SerializeField] private bool loopPatrol = true;
    [SerializeField] private float waitAtWaypoint = 0.5f;

    [Header("Velocidades")]
    [SerializeField] private float patrolSpeed = 2.0f;
    [SerializeField] private float chaseSpeed = 3.5f;

    [Header("Persecución")]
    [SerializeField] private float forgetTargetAfterSeconds = 4f; // si no lo ve en este tiempo, vuelve a patrullar

    [Header("Rotación")]
    [SerializeField] private float rotateTowardsTargetSpeed = 9f;

    private NavMeshAgent _agent;
    private EnemyPerception _perception;
    private EnemyAttack _attack;

    private State _state;
    private int _currentWP;
    private float _waitTimer;
    private float _lastTimeSawTarget;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _perception = GetComponent<EnemyPerception>();
        _attack = GetComponent<EnemyAttack>();
    }

    private void Start()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        // Setup inicial
        _agent.stoppingDistance = Mathf.Max(_attack.AttackRange * 0.8f, 1.0f);
        _agent.speed = patrolSpeed;
        _agent.autoBraking = true;

        _state = State.Patrol;
        GoToNextWaypoint();
    }

    private void Update()
    {
        // Sensado de jugador
        bool seesPlayer = player != null && _perception.CanSeeTarget(player);
        if (seesPlayer) _lastTimeSawTarget = Time.time;

        switch (_state)
        {
            case State.Patrol:
                PatrolUpdate(seesPlayer);
                break;
            case State.Chase:
                ChaseUpdate(seesPlayer);
                break;
            case State.Attack:
                AttackUpdate(seesPlayer);
                break;
        }
    }

    private void PatrolUpdate(bool seesPlayer)
    {
        _agent.speed = patrolSpeed;

        if (seesPlayer)
        {
            _state = State.Chase;
            return;
        }

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.05f)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= waitAtWaypoint)
            {
                _waitTimer = 0f;
                GoToNextWaypoint();
            }
        }
    }

    private void ChaseUpdate(bool seesPlayer)
    {
        _agent.speed = chaseSpeed;

        if (player == null)
        {
            // sin target → volver a patrulla
            _state = State.Patrol;
            GoToNearestWaypointOrContinue();
            return;
        }

        Vector3 destination;
        if (seesPlayer)
        {
            destination = player.position;
        }
        else
        {
            // ir a última posición recordada mientras no expire el “recuerdo”
            if (Time.time - _lastTimeSawTarget <= forgetTargetAfterSeconds)
                destination = _perception.LastSensedPosition;
            else
            {
                _state = State.Patrol;
                GoToNearestWaypointOrContinue();
                return;
            }
        }

        _agent.SetDestination(destination);

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= _attack.AttackRange + 0.15f)
        {
            _state = State.Attack;
            _agent.isStopped = true;
        }
        else
        {
            _agent.isStopped = false;
        }
    }

    private void AttackUpdate(bool seesPlayer)
    {
        if (player == null)
        {
            _state = State.Patrol;
            _agent.isStopped = false;
            GoToNearestWaypointOrContinue();
            return;
        }

        // mirar hacia el jugador
        Vector3 to = player.position - transform.position; to.y = 0f;
        if (to.sqrMagnitude > 0.001f)
        {
            Quaternion look = Quaternion.LookRotation(to);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, rotateTowardsTargetSpeed * Time.deltaTime);
        }

        float dist = Vector3.Distance(transform.position, player.position);

        // si está lejos, volver a perseguir
        if (dist > _attack.AttackRange + 0.25f)
        {
            _state = State.Chase;
            _agent.isStopped = false;
            return;
        }

        // atacar (respeta cooldown)
        _attack.TryAttack(player);

        // Si dejamos de verlo por mucho tiempo, soltar
        if (!seesPlayer && Time.time - _lastTimeSawTarget > forgetTargetAfterSeconds)
        {
            _state = State.Patrol;
            _agent.isStopped = false;
            GoToNearestWaypointOrContinue();
        }
    }

    private void GoToNextWaypoint()
    {
        if (patrolPoints == null || patrolPoints.Count == 0) return;

        _agent.SetDestination(patrolPoints[_currentWP].position);

        if (loopPatrol)
            _currentWP = (_currentWP + 1) % patrolPoints.Count;
        else
            _currentWP = Mathf.Min(_currentWP + 1, patrolPoints.Count - 1);
    }

    private void GoToNearestWaypointOrContinue()
    {
        if (patrolPoints == null || patrolPoints.Count == 0) return;

        // Elegir el WP más cercano para retomar patrulla con naturalidad
        int nearest = 0;
        float best = float.MaxValue;
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            float d = Vector3.SqrMagnitude(transform.position - patrolPoints[i].position);
            if (d < best) { best = d; nearest = i; }
        }
        _currentWP = nearest;
        GoToNextWaypoint();
    }
}
