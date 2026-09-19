using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;

    [Header("Rangos")]
    public float detectionRange = 8f;
    public float attackRange = 2f;
    public float maxChaseRange = 15f;
    public float stopDistance = 1.5f;

    [Header("Movimiento")]
    public float moveSpeed = 3f;

    [Header("Ataque")]
    public int damage = 10;
    public float attackInterval = 1.5f;

    [Header("Obstáculos")]
    public LayerMask obstacleLayer;
    public float extraDetectionDistance = 0.08f;
    public float sideCheckDistance = 0.8f;

    [Header("Visión")]
    public LayerMask visionObstacleLayer;
    public Transform eyePoint;
    public float eyeHeight = 0f;

    private Rigidbody2D rb;
    private Collider2D enemyCollider;

    private float attackCooldown;
    private Vector2 movementDirection;

    private readonly RaycastHit2D[] hits =
        new RaycastHit2D[10];

    private enum State
    {
        Idle,
        Chase,
        Attack
    }

    private State currentState = State.Idle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode =
            CollisionDetectionMode2D.Continuous;
        rb.interpolation =
            RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        BuscarJugador();

        if (player == null)
        {
            movementDirection = Vector2.zero;
            currentState = State.Idle;
            return;
        }

        float distance = Vector2.Distance(
            rb.position,
            player.position
        );

        bool playerVisible = PuedeVerAlJugador();

        DecidirEstado(distance, playerVisible);

        switch (currentState)
        {
            case State.Idle:
                movementDirection = Vector2.zero;
                break;

            case State.Chase:
                CalcularDireccionDePersecucion(distance);
                break;

            case State.Attack:
                movementDirection = Vector2.zero;
                AtacarJugador();
                break;
        }

        GirarHaciaJugador();
    }

    private void FixedUpdate()
    {
        if (currentState != State.Chase)
            return;

        if (movementDirection == Vector2.zero)
            return;

        MoverYEsquivarObstaculos();
    }

    private void BuscarJugador()
    {
        if (player != null)
            return;

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    private void DecidirEstado(
        float distance,
        bool playerVisible
    )
    {
        bool insideMaxRange =
            distance <= maxChaseRange;

        bool insideDetectionRange =
            distance <= detectionRange;

        if (!insideMaxRange || !playerVisible)
        {
            currentState = State.Idle;
            return;
        }

        if (distance <= attackRange)
        {
            currentState = State.Attack;
        }
        else if (insideDetectionRange)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Idle;
        }
    }

    private void CalcularDireccionDePersecucion(
        float distance
    )
    {
        if (distance <= stopDistance)
        {
            movementDirection = Vector2.zero;
            return;
        }

        Vector2 direction =
            (Vector2)player.position - rb.position;

        movementDirection = direction.normalized;
    }

    private bool PuedeVerAlJugador()
    {
        Vector2 start;

        if (eyePoint != null)
        {
            start = eyePoint.position;
        }
        else
        {
            start = rb.position +
                Vector2.up * eyeHeight;
        }

        Vector2 end = player.position;

        RaycastHit2D hit = Physics2D.Linecast(
            start,
            end,
            visionObstacleLayer
        );

        // Si el Linecast no golpea ningún obstáculo,
        // el enemigo puede ver al jugador.
        return hit.collider == null;
    }

    private void MoverYEsquivarObstaculos()
    {
        float moveDistance =
            moveSpeed * Time.fixedDeltaTime;

        Vector2 direction = movementDirection;

        bool blocked = HayObstaculo(
            direction,
            moveDistance + extraDetectionDistance
        );

        if (!blocked)
        {
            Mover(direction, moveDistance);
            return;
        }

        Vector2 leftDirection = new Vector2(
            -direction.y,
            direction.x
        ).normalized;

        if (!HayObstaculo(
            leftDirection,
            sideCheckDistance
        ))
        {
            Mover(leftDirection, moveDistance);
            return;
        }

        Vector2 rightDirection = new Vector2(
            direction.y,
            -direction.x
        ).normalized;

        if (!HayObstaculo(
            rightDirection,
            sideCheckDistance
        ))
        {
            Mover(rightDirection, moveDistance);
            return;
        }

        Vector2 backwardDirection = -direction;

        if (!HayObstaculo(
            backwardDirection,
            moveDistance
        ))
        {
            Mover(backwardDirection, moveDistance);
        }
    }

    private bool HayObstaculo(
        Vector2 direction,
        float distance
    )
    {
        int hitCount = enemyCollider.Cast(
            direction,
            hits,
            distance,
            true
        );

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D hitCollider = hits[i].collider;

            if (hitCollider == null)
                continue;

            int hitLayer =
                1 << hitCollider.gameObject.layer;

            bool isObstacle =
                (obstacleLayer.value & hitLayer) != 0;

            if (isObstacle)
                return true;
        }

        return false;
    }

    private void Mover(
        Vector2 direction,
        float distance
    )
    {
        Vector2 newPosition =
            rb.position + direction * distance;

        rb.MovePosition(newPosition);
    }

    private void AtacarJugador()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }

        if (player == null)
            return;

        EntityHealth EntityHealth =
            player.GetComponent<EntityHealth>();

        if (EntityHealth != null)
        {
            EntityHealth.TakeDamage(damage);
        }

        attackCooldown = attackInterval;
    }

    private void GirarHaciaJugador()
    {
        if (player == null)
            return;

        Vector2 direction =
            player.position - transform.position;

        if (Mathf.Abs(direction.x) < 0.01f)
            return;

        Vector3 scale = transform.localScale;

        if (direction.x > 0f)
            scale.x = Mathf.Abs(scale.x);
        else
            scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        if (player != null)
        {
            Vector3 start = eyePoint != null
                ? eyePoint.position
                : transform.position;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(start, player.position);
        }
    }
}