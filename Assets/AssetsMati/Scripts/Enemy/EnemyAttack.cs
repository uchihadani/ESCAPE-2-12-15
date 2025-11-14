using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 15;
    [SerializeField] private float attackRange = 1.8f;
    [SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private Transform attackOrigin; // opcional: donde “sale” el golpe

    private float _nextAttackTime;

    Transform Origin => attackOrigin != null ? attackOrigin : transform;

    public float AttackRange => attackRange;
    public bool IsOnCooldown => Time.time < _nextAttackTime;

    /// Llamar en el estado de Ataque. Devuelve true si ejecutó ataque.
    public bool TryAttack(Transform target)
    {
        if (IsOnCooldown || target == null) return false;

        float dist = Vector3.Distance(Origin.position, target.position);
        if (dist > attackRange) return false;

        // Si tenés animaciones, aquí disparás el trigger y haces el daño via Animation Event
        // Para prototipo aplicamos el daño directo:
        ApplyDamage(target);

        _nextAttackTime = Time.time + attackCooldown;
        return true;
    }

    // Podés llamar este método desde un Animation Event si querés que el daño “caiga” en un frame preciso
    public void AnimationEvent_DealDamage()
    {
        // Buscar al jugador más cercano en el rango (por si se movió)
        Collider[] hits = Physics.OverlapSphere(Origin.position, attackRange);
        foreach (var h in hits)
        {
            if (h.CompareTag("Player"))
            {
                ApplyDamage(h.transform);
                break;
            }
        }
    }

    private void ApplyDamage(Transform target)
    {
        if (target.TryGetComponent<IDamageable>(out var dmg))
        {
            dmg.TakeDamage(damage);
        }
        else
        {
            // Busca en padres por si el Health está arriba
            var d = target.GetComponentInParent<IDamageable>();
            if (d != null) d.TakeDamage(damage);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Origin.position, attackRange);
    }
#endif
}
