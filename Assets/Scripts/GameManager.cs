using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    private int score = 0; // Score variable
    public static GameManager Instance { get; private set; } // Singleton instance
    [SerializeField] private TMP_Text scoreText;

    public int Score { get { return score; } } // Read-only access to the score

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
        UpdateScoreUI();

    }
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogError("Score Text UI element not assigned!");
        }
    }
}
