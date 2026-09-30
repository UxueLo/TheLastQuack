using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    

    [SerializeField] public int maxHealth = 100;
    private int currentHealth;
    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (CompareTag("Player"))
        {
            if (gameManager != null && gameManager.hasPlayerHealth)
            {
                currentHealth=gameManager.playerHealth;
            }
            else
            {
                currentHealth = maxHealth;
                if(gameManager!=null)
                {
                    gameManager.playerHealth = currentHealth;
                    gameManager.hasPlayerHealth = true;
                }
            }
        }
        else
        {
            //VER SI IMPLEMENTAR LOGICA PARA ENEMIGOS, BOTS...
            currentHealth = maxHealth;
        }
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log("Player health: " + currentHealth);

        
        if (CompareTag("Player") && gameManager != null)
        {
            gameManager.playerHealth = currentHealth;
            gameManager.hasPlayerHealth = true;
        }

        if (currentHealth <= 0) {
           Die();
        }

    }


    public void Heal(int amount) //CURACION, POR VER SI SE IMPLEMENTA
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log("Player health: " + currentHealth);
        
        if (CompareTag("Player") && gameManager != null)
        {
            gameManager.playerHealth = currentHealth;
            gameManager.hasPlayerHealth = true;
        }

    }

    void Die() 
    {
        Debug.Log("Player died");
    }
}
