using System.Collections.Generic;
using UnityEngine;

public class RatNest : MonoBehaviour
{
    public GameObject ratPrefab;
    public RatCatcher RatCatcher;
    NavManager navigator;
    [SerializeField]
    List<Rat> rats = new List<Rat>();
    int maxRat = 64;
    public Vector3[] points;

    private void Start()
    {
        navigator = new NavManager();
        RatCatcher = FindObjectOfType<RatCatcher>();
        for (int i = 0; i < maxRat; i++)
        {
            createRat();
        }
    }

    void createRat()
    {
        // create the rat object
        GameObject newRatRef = Instantiate(ratPrefab, navigator.generateRandomPoint(true), ratPrefab.transform.rotation);
        Rat rat = newRatRef.GetComponent<Rat>();

        // set up the variables
        rat.setNest(this);
        rat.navigator = navigator;

        // set priority, this is kept to a small range to not be noticable
        rat.agent.avoidancePriority = 50 + Random.Range(-5, 5);

        // add to list
        rats.Add(rat);
    }

    public void killRat(Rat rat)
    {
        // get rid of rat from list and destroy it
        rats.Remove(rat);
        Object.Destroy(rat.gameObject);

        // create a new rat!
        createRat();
    }
}
