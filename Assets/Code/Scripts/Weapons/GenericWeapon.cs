using UnityEngine;

public class GenericWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.2f;
    public float speed = 2f;

    private float nextFireTime = 0f;

    private void Update()
    {
        RotateWeapon();

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void RotateWeapon()
    {
        // Obtener la posición del ratón en la pantalla
        Vector3 mousePosition =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // En un juego 2D no necesitamos modificar el eje Z
        mousePosition.z = 0f;

        // Dirección desde el arma hacia el ratón
        Vector3 direction = mousePosition - transform.position;

        // Calcular el ángulo de rotación
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Girar el arma
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * bulletSpeed * speed;
        }
    }
}