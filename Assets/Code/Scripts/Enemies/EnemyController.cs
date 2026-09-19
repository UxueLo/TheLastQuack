using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform target; //jugador
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float detectionRange = 20f;
    [SerializeField] int attackDamage = 10;
    [SerializeField] float attackInterval = 1.5f;
    [SerializeField] float attackRange = 1f;
    //[SerializeField] float stopRange = PONER LIMITE DE SEGUIMIENTO

    UnityEngine.AI.NavMeshAgent agent;
    private float attackCoolDown = 0;


    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {       
        if (attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime; //actualizamos la cuenta atras
            return;
        }
           float distance = Vector2.Distance(transform.position, target.position); //distancia con el jugador
           agent.SetDestination(target.position);

           if (distance > detectionRange)
           {
                return; //no hace nada
           }
           else if (distance <= detectionRange)
           {
                if (distance <= attackRange){
                    attackPlayer(); // ataca al jugador
                }
                return;
           }

    }

    private void attackPlayer()
    {
        
        EntityHealth entityHealth = target.GetComponent<EntityHealth>();
        if(entityHealth != null){
            entityHealth.TakeDamage(attackDamage);
            Debug.Log("Tu vida: " + entityHealth);
        }else{
            Debug.Log("Has muerto pringui");
        }
        attackCoolDown = attackInterval;
    }

}
