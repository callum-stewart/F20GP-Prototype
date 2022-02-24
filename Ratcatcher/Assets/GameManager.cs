using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject ratNestPrefab;
    public AudioManager audioManager;
    public static GameManager instance;
    public bool hasKeyCard = true;
    public LightSwitch ReceptionLight;

    private Vector3[] points = {
        new Vector3(5.5f, .15f, 0f),    // Reception
        new Vector3(-11f, .15f, 2f),    // Breakroom
        new Vector3(3f, .15f, 15f),     // GN Toilet
        new Vector3(-8f, .15f, 16f),     // Janitorial Cupboard
        new Vector3(-3f, .15f, 24f),    // Offices 
        new Vector3(-2f, .15f, 36f),    // lab
        new Vector3(17f, .15f, 55f),    // holding
        new Vector3(-15f, .15f, 51f),     // head office
        new Vector3(-7f, .15f, 44f),    // Delivery ROAM ONLY
        new Vector3(10f, .15f, 24f),     // Hallway B ROAM ONLY
        new Vector3(7f, .15f, 57f),     // Hallway C ROAM ONLY
        new Vector3(-3f, .15f, 14f),     // Prep Room ROAM ONLY
        new Vector3(19f, .15f, 38f)     // security ROAM ONLY
    };

    // make sure only once game manager instance that exists
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // change the current scene
    public void ChangeScene(int scene)
    {
        // attempt to load scene
        SceneManager.LoadScene(scene);

        // wait for the scene to be loaded
        if (SceneManager.GetActiveScene().buildIndex != scene)
            StartCoroutine(waitForSceneLoad(scene));
    }

    // waits for the appropraite scene to be loaded before set up is started
    IEnumerator waitForSceneLoad(int scene)
    {
        while (SceneManager.GetActiveScene().buildIndex != scene)
            yield return null;

        if (SceneManager.GetActiveScene().buildIndex == scene)
            sceneSetup(scene);
    }

    void sceneSetup(int scene)
    {
        //stopAllSound();
        switch (scene)
        {
            case 0:
                mainMenu();
                break;
            case 1:
                theLabs();
                break;
            case 2:
                gameOver();
                break;
            case 3:
                winScreen();
                break;
            default:
                break;
        }
    }

    private void stopAllSound()
    {
        foreach (Sound s in audioManager.sounds)
            audioManager.Stop(s.name);
    }

    private void mainMenu()
    {
        // unlock cursor
        Cursor.lockState = CursorLockMode.None;
    }

    // play button pressed
    private void theLabs()
    {
        ReceptionLight = GameObject.Find("Reception Light Switch").GetComponent<LightSwitch>();

        for (int i = 0; i < points.Length - 4; i++)
        {
            // create the nest object
            GameObject newRatRef = Instantiate(ratNestPrefab, points[i], ratNestPrefab.transform.rotation);
            RatNest nest = newRatRef.GetComponent<RatNest>();
            nest.points = points;
        }
    }

    private void gameOver()
    {
        // unlock cursor
        Cursor.lockState = CursorLockMode.None;
        // play sound effect
        audioManager.Play("Scream");
    }

    private void winScreen()
    {
        // unlock cursor
        Cursor.lockState = CursorLockMode.None;
    }

    public void checkWinCondition()
    {
        if (ReceptionLight.isOn)
            ChangeScene(3);
        else
            ChangeScene(2);
    }
}
