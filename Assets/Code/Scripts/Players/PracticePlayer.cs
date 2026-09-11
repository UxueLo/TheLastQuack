using UnityEngine;
using UnityEngine.InputSystem;

public class PracticePlayer: MonoBehaviour
{
    [Header("Movimiento")] //texto para una mejor visibilidad
    [SerializeField] private float moveSpeed = 8f; //para que en el inspector se pueda ver aunque sea private

    [Header("Referencias")] //ref3rencias
    private Rigidbody2D rb;
    [SerializeField] private Camera mainCamera;

    private Vector2 moveInput; //estructura que almacena dos numeros flotantes, se usa para direcciones, posiciones, velocidades o dimensiones en 2D
    private Vector2 mousePosition;

    private void Update()
    {
        //Lectura del teclado, con input system
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized; //mantiene la misma velocidad al moverse

        //Obtener posicion del raton
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime); //Aplicar movimiento fisico constante sin traspasar paredes

        RotateTowardsMouse();
    }

    private void RotateTowardsMouse()
    {
        Vector2 lookDirection = mousePosition - rb.position; // Calcular el vector de direccion desde el jugador hacia el raton

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg; // Calcular el angulo en grados usando la tangente del vector

        rb.rotation = angle - 90f; //Ajuste de 90° segun la orientacion original del sprite
    }

    private void Awake() //solucionar problema de camra no asignada
    {
        rb = GetComponent<Rigidbody2D>();
    
        // Si la casilla del Inspector está vacía, la busca automáticamente por Tag
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
}