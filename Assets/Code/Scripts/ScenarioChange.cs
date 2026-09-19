using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioChange : MonoBehaviour
{
    public string nombreSiguienteEscena;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(nombreSiguienteEscena);
        }
    }
}
