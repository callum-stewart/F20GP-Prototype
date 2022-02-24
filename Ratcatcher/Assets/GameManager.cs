using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject ratNestPrefab;
    private Vector3[] spawnPoints = {
        new Vector3(5.5f, .15f, 0f),    // Reception
        new Vector3(-11f, .15f, 2f),    // Breakroom
        new Vector3(9.5f, .15f, 18.5f), // Offices
        new Vector3(9.5f, .15f, 31.5f), // Offices 2nd
        new Vector3(18f, .15f, 37.5f),  // Security
        new Vector3(5.5f, .15f, 38.5f), // Holding
        new Vector3(-8f, .15f, 46f),    // Delivery
        new Vector3(-14.5f, .15f, 53f)    // Head Office
    };

    public void Start()
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            // create the nest object
            GameObject newRatRef = Instantiate(ratNestPrefab, spawnPoints[i], ratNestPrefab.transform.rotation);
            RatNest nest = newRatRef.GetComponent<RatNest>();
        }
        
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
