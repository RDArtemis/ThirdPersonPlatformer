using UnityEngine;

public class Coin : MonoBehaviour
{
    public int pointsValue = 1; // Point Value of Coin
    public float rotationSpeed = 100f; // Speed of rotation


    void Update()
    {
        // Rotate the coin
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(pointsValue); 
            Destroy(gameObject);
        }
    }
}

