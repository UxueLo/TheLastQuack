using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class Nexu : MonoBehaviour
{
    [Header("Configuration")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    private Vector2 lastMoveInput = new Vector2(0f, -1f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //recibir la informacion de los componentes de unity
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Evitar que personaje se caiga
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //Recoher datos de teclado
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(inputX, inputY).normalized;

        //Girar el personaje a izquierda o derecha
        if(moveInput != Vector2.zero)
        {
            lastMoveInput = moveInput;
        }

        //enviar info a animator
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetFloat("LastMoveX", lastMoveInput.x);
        animator.SetFloat("LastMoveY", lastMoveInput.y);
        animator.SetBool("isMoving", moveInput != Vector2.zero);
    }

    void FixedUpdate()
    {
        //mover al perosnaje usando rigidbody
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
