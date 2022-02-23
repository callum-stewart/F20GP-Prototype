using System.Collections.Generic;
using UnityEngine;

public class RatNest : MonoBehaviour
{
    public GameObject ratPrefab;
    public RatCatcher RatCatcher;
    [SerializeField]
    List<Rat> rats = new List<Rat>();
    Rat leader;

    int maxRat = 30;
    private Vector3[] spawnPoints = {
        new Vector3(5.5f, .15f, 0f),    // Reception
        new Vector3(9.5f, .15f, 18.5f), // Offices
        new Vector3(9.5f, .15f, 31.5f), // Offices 2nd
        new Vector3(18f, .15f, 37.5f),  // Security
        new Vector3(5.5f, .15f, 38.5f), // Holding
        new Vector3(-8f, .15f, 46f),    // Delivery
        new Vector3(-14.5f, .15f, 53f)    // Head Office
    };
    private Vector3[] roamingPoints = {
        new Vector3(5.5f, .15f, 0f),    // Reception
        new Vector3(9.5f, .15f, 18.5f), // Offices
        new Vector3(9.5f, .15f, 31.5f), // Offices 2nd
        new Vector3(18f, .15f, 37.5f),  // Security
        new Vector3(5.5f, .15f, 38.5f), // Holding
        new Vector3(-8f, .15f, 46f),    // Delivery
        new Vector3(-14.5f, .15f, 53f)    // Head Office
    };

    private void Awake()
    {
        RatCatcher = FindObjectOfType<RatCatcher>();
        for (int i = 0; i < maxRat; i++)
        {
            createRat();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void createRat()
    {
        // create the rat object
        GameObject newRatRef = Instantiate(ratPrefab, generateRandomPoint(spawnPoints), ratPrefab.transform.rotation);
        Rat rat = newRatRef.GetComponent<Rat>();

        // set up the variables
        rat.setNest(this);
        if (rats.Count == 0)
        {
            // first rat created is the leader
            rat.isLeader = true;
            leader = rat;
        }

        // add to list
        rats.Add(rat);
    }

    public void killRat(Rat rat)
    {
        
        // replace leader if required
        if (rat.isLeader)
        {
            leader = rats.Find((rat) => !rat.isLeader);
            leader.isLeader = true;
        }


        // get rid of rat from list and destroy it
        rats.Remove(rat);
        Object.Destroy(rat.gameObject);

        // create a new rat!
        createRat();
    }

    public Vector3 getInstruction(bool isLeader)
    {
        return (isLeader ? generateRandomPoint(roamingPoints) : leader.transform.position);
    }

    private Vector3 generateRandomPoint(Vector3[] points)
    {
        int randomIndex = Random.Range(0, points.Length);
        return points[randomIndex];
    }
}
