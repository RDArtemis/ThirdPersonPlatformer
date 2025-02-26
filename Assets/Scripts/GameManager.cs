using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    private int score = 0; // Score variable
    public static GameManager Instance { get; private set; } // Singleton instance



    public void AddScore(int points)
    {
        score += points;
        
    }
}
