using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    

    [SerializeField] public int maxHealth = 100;
    private int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log("Player health: " + currentHealth);

        if (currentHealth <= 0) {
           Die();
        }
    }

    void Die() 
    {
        Debug.Log("Player died");
    }
}
