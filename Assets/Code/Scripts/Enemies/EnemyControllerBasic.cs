using UnityEngine;

public class EnemyControllerBasic : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;

    [Header("Distancias")]
    public float detectionRange = 8f;
    public float attackRange = 2f;
    public float stopDistance = 1.5f;

    [Header("Movimiento")]
    public float moveSpeed = 3f;

    [Header("Ataque")]
    public int damage = 10;
    public float attackInterval = 1.5f;

    private float attackCooldown;

    private void Update()
    {
        // Buscar automáticamente al jugador si no está asignado
        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                return;
            }
        }

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        // Si está demasiado lejos, no hace nada
        if (distance > detectionRange)
        {
            return;
        }

        // Si está cerca, ataca
        if (distance <= attackRange)
        {
            AttackPlayer();
            return;
        }

        // Si todavía no está a distancia de ataque, persigue
        if (distance > stopDistance)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        Vector2 direction =
            (player.position - transform.position).normalized;

        transform.position +=
            (Vector3)(direction * moveSpeed * Time.deltaTime);

        FacePlayer();
    }

    private void AttackPlayer()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }

        EntityHealth EntityHealth =
            player.GetComponent<EntityHealth>();

        if (EntityHealth != null)
        {
            EntityHealth.TakeDamage(damage);
        }

        attackCooldown = attackInterval;
    }

    private void FacePlayer()
    {
        if (player == null)
            return;

        Vector2 direction =
            player.position - transform.position;

        Vector3 scale = transform.localScale;

        if (direction.x > 0f)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else if (direction.x < 0f)
        {
            scale.x = -Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }
}
