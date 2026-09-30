using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playerHealth;
    public bool hasPlayerHealth;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}