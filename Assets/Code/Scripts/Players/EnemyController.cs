using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Referencias
    public Transform player;

    // Rangos
    public float detectionRange = 8f;   // rango para empezar a perseguir
    public float attackRange = 2f;      // rango para atacar
    public float maxChaseRange = 15f;   // opcional: rango máximo de persecución

    // Movimiento
    public float moveSpeed = 3f;
    public float stopDistance = 1.5f;   // distancia a la que se para antes de atacar

    private enum State { Idle, Chase, Attack }
    private State currentState = State.Idle;

    private float attackCooldown = 0f;
    public float attackInterval = 1.5f; // tiempo entre ataques

    // Color
    public Color newColor = Color.red;

    void Update()
    {
        if (player == null)
        {
            // Si no hay jugador asignado, lo buscamos por tag
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            else return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        // Decidir estado
        if (distance <= attackRange && distance <= maxChaseRange)
        {
            currentState = State.Attack;
        }
        else if (distance <= detectionRange && distance <= maxChaseRange)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Idle;
        }

        // Ejecutar comportamiento según estado
        switch (currentState)
        {
            case State.Idle:
                // Opcional: patrullar, mirar alrededor, etc.
                break;

            case State.Chase:
                ChasePlayer(distance);
                break;

            case State.Attack:
                AttackPlayer();
                break;
        }
    }

    void ChasePlayer(float distance)
    {
        // Si ya está muy cerca, no seguir acercándose (evita solaparse)
        if (distance > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        }

        // Opcional: orientar el sprite hacia el jugador
        FacePlayer();
    }

    void AttackPlayer()
    {
        Renderer rend = GetComponent<Renderer>();
        // Cooldown para no atacar cada frame
        if (attackCooldown > 0f)
        {
            rend.material.color = newColor;
            attackCooldown -= Time.deltaTime;
            return;
        }

        rend.material.color = Color.white;

        // Aquí pones tu lógica de daño
        // Ejemplo: buscar componente EntityHealth en el jugador
        var EntityHealth = player.GetComponent<EntityHealth>();
        if (EntityHealth != null)
        {
            EntityHealth.TakeDamage(10); // ejemplo
        }

        // Reiniciar cooldown
        attackCooldown = attackInterval;

        // Opcional: activar animación de ataque
        // animator.SetTrigger("Attack");
    }

    void FacePlayer()
    {
        // Para 2D: voltear sprite según dirección
        Vector2 direction = player.position - transform.position;
        if (direction.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}

