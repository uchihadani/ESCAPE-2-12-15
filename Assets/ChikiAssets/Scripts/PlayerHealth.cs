using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
    }

   public void TakeDamage(int amount)
    {
        currentHealth -= amount;
    }
}
