using System.Collections.Generic;
using UnityEngine;

public class RatNest : MonoBehaviour
{
    public GameObject ratPrefab;
    public RatCatcher RatCatcher;
    [SerializeField]
    List<Rat> rats = new List<Rat>();
    int maxRat = 36;
    public Vector3[] points;

    private void Start()
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
        GameObject newRatRef = Instantiate(ratPrefab, generateRandomPoint(true), ratPrefab.transform.rotation);
        Rat rat = newRatRef.GetComponent<Rat>();

        // set up the variables
        rat.setNest(this);
        if (rats.Count == 0)
        {
            // first rat created is the leader
            //rat.isLeader = true;
            //leader = rat;
        }

        // add to list
        rats.Add(rat);
    }

    // gets the mass of this nest group through average position
    // this is from the current rats POV
    public Vector3 getMass(Rat currentRat)
    {
        // get the sum of all the positions, except current
        Vector3 sumOfPos = new Vector3();
        foreach (Rat r in rats)
            if(currentRat != r)
                sumOfPos += r.transform.position;

        // count is used due to possibility of missing rats
        return sumOfPos / (rats.Count-1);
    }

    public Vector3 getDistance(Rat currentRat)
    {
        // get the sum of all the positions, except current
        Vector3 distance = new Vector3();
        foreach (Rat r in rats)
            if (currentRat != r)
                if ((r.transform.position - currentRat.transform.position).magnitude < 5)
                    distance = distance - (r.transform.position - currentRat.transform.position);

        // count is used due to possibility of missing rats
        return distance;
    }

    // get the average velocity of the nest
    // this is from the current rats POV
    public Vector3 getVelocity(Rat currentRat)
    {
        // get the sum of all the positions, except current
        Vector3 sumOfVel = new Vector3();
        foreach (Rat r in rats)
            if (currentRat != r)
                sumOfVel += r.velocity;

        // count is used due to possibility of missing rats
        return sumOfVel / (rats.Count - 1);
    }
    public void killRat(Rat rat)
    {
        
        /*// replace leader if required
        if (rat.isLeader)
        {
            //leader = rats.Find((rat) => !rat.isLeader);
            //leader.isLeader = true;
        }*/


        // get rid of rat from list and destroy it
        rats.Remove(rat);
        Object.Destroy(rat.gameObject);

        // create a new rat!
        createRat();
    }

    public Vector3 getInstruction()
    {
        return generateRandomPoint(false);
    }

    
    private Vector3 generateRandomPoint(bool isSpawn)
    {
        int rIndex = isSpawn ? Random.Range(0, points.Length - 4) : Random.Range(0, points.Length);
        return points[rIndex];
    }
}
