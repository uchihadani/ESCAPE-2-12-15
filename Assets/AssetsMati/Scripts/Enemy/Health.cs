using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    public int CurrentHealth { get; private set; }

    public UnityEvent onDamaged;
    public UnityEvent onDied;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        onDamaged?.Invoke();

        if (CurrentHealth == 0)
        {
            onDied?.Invoke();
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject, 2f);
    }
}
