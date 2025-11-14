using UnityEngine;

public class EnemyPerception : MonoBehaviour
{
    [Header("Vista")]
    [SerializeField] private Transform eyePoint;     // Asigná un hijo "Eye" o deja null para usar transform
    [SerializeField] private float viewRadius = 12f;
    [SerializeField, Range(1f, 360f)] private float viewAngle = 120f;
    [SerializeField] private LayerMask obstacleMask; // Obstacles

    [Header("Oído")]
    [SerializeField] private float hearingRadius = 4f;

    public Vector3 LastSensedPosition { get; private set; }
    public float LastSensedTime { get; private set; }

    Transform Eye => eyePoint != null ? eyePoint : transform;

    public bool CanSeeTarget(Transform target)
    {
        if (target == null) return false;

        Vector3 origin = Eye.position;
        Vector3 toTarget = (target.position - origin);
        float dist = toTarget.magnitude;

        // “Oído”: si está muy cerca, lo detecta aunque no esté en el FOV
        if (dist <= hearingRadius)
        {
            Remember(target.position);
            return true;
        }

        if (dist > viewRadius) return false;

        Vector3 dir = toTarget.normalized;
        float angle = Vector3.Angle(Eye.forward, dir);
        if (angle > viewAngle * 0.5f) return false;

        // Occlusión por obstáculos
        if (Physics.Raycast(origin, dir, out RaycastHit hit, dist, obstacleMask))
            return false;

        Remember(target.position);
        return true;
    }

    public void Remember(Vector3 pos)
    {
        LastSensedPosition = pos;
        LastSensedTime = Time.time;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Radios
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyePoint ? eyePoint.position : transform.position, viewRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(eyePoint ? eyePoint.position : transform.position, hearingRadius);

        // FOV
        Vector3 forward = (eyePoint ? eyePoint.forward : transform.forward);
        Vector3 left = Quaternion.Euler(0, -viewAngle * 0.5f, 0) * forward;
        Vector3 right = Quaternion.Euler(0, viewAngle * 0.5f, 0) * forward;

        Gizmos.color = Color.green;
        Vector3 origin = eyePoint ? eyePoint.position : transform.position;
        Gizmos.DrawLine(origin, origin + left * viewRadius);
        Gizmos.DrawLine(origin, origin + right * viewRadius);
    }
#endif
}
