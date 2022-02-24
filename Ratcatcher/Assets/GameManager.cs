using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject ratNestPrefab;
    private Vector3[] points = {
        new Vector3(5.5f, .15f, 0f),    // Reception
        new Vector3(-11f, .15f, 2f),    // Breakroom
        new Vector3(2f, .15f, 12f),     // GN Toilet
        new Vector3(9f, .15f, 16f),     // Janitorial Cupboard
        new Vector3(-4f, .15f, 25f),    // Offices 
        new Vector3(-3f, .15f, 36f),    // lab
        new Vector3(17f, .15f, 56f),    // holding
        new Vector3(16f, .15f, 55f),     // head office
        new Vector3(-7f, .15f, 44f),    // Delivery ROAM ONLY
        new Vector3(7f, .15f, 58f),     // Hallway C ROAM ONLY
        new Vector3(5f, .15f, 14f),     // Prep Room ROAM ONLY
        new Vector3(17f, .15f, 38f)     // security ROAM ONLY
    };

    public void Start()
    {
        for(int i = 0; i < points.Length-4; i++)
        {
            // create the nest object
            GameObject newRatRef = Instantiate(ratNestPrefab, points[i], ratNestPrefab.transform.rotation);
            RatNest nest = newRatRef.GetComponent<RatNest>();
            nest.points = points;
        }
        
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
