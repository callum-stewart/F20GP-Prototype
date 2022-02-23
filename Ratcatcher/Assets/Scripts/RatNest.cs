using System.Collections.Generic;
using UnityEngine;

public class RatNest : MonoBehaviour
{
    public GameObject ratPrefab;
    List<Rat> rats = new List<Rat>();
    Rat leader;

    int maxRat = 12;
    Vector3 spawnPoint = new Vector3(-2.5f, .1f, -1.5f);
    private Vector3[] roamingPoints = {
        new Vector3(5.5f, .1f, 0f),    // Reception
        new Vector3(9.5f, .1f, 18.5f), // Offices
        new Vector3(9.5f, .1f, 31.5f), // Offices 2nd
        new Vector3(18f, .1f, 37.5f),  // Security
        new Vector3(5.5f, .1f, 38.5f), // Holding
        new Vector3(-8f, .1f, 46f),    // Delivery
        new Vector3(-14.5f, .1f, 53f)    // Head Office
    };

    private void Awake()
    {
        for(int i = 0; i < maxRat; i++)
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
        GameObject newRatRef = Instantiate(ratPrefab, spawnPoint, ratPrefab.transform.rotation);
        Rat rat = newRatRef.GetComponent<Rat>();

        // set up the variables
        rat.setNest(this);
        if (rats.Count == 0)
        {    // first rat created is the leader
            rat.isLeader = true;
            leader = rat;
        }

        // add to list
        rats.Add(rat);
    }

    public Vector3 getInstruction(bool isLeader)
    {
        return (isLeader ? generateRandomPoint() : leader.transform.position);
    }

    private Vector3 generateRandomPoint()
    {
        int randomIndex = Random.Range(0, roamingPoints.Length);
        return roamingPoints[randomIndex];
    }
}
