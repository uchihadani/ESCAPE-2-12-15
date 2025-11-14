using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    [SerializeField] private AudioClip recibirDaño;

    void Start()
    {
        currentHealth = maxHealth;
    }

   public void TakeDamage(int amount)
    {
        SoundManager.instance.PlaySound(recibirDaño);
        currentHealth -= amount;
    }
}
    