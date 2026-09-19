using UnityEngine;

public class Bullet_Controll : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag ("Collision"))
        {
            Destroy(gameObject);
        }
    }
}
