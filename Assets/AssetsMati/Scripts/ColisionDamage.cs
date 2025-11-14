using UnityEngine;

public class ColisionDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);
            Debug.Log($"{other.name} recibió {damageAmount} de daño por colisión.");
        }
    }
}
