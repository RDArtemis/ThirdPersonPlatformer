using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    private int score = 0; // Score variable
    public static GameManager Instance { get; private set; } // Singleton instance

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Optional: Keep GameManager across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }
    }

    public void AddScore(int points)
    {
        score += points;
        
    }
}
