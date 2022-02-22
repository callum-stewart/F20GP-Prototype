using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatNestController : MonoBehaviour
{
    public GameObject ratPrefab;
    List<GameObject> rats = new List<GameObject>();
    int maxRat = 12;
    Vector3 spawnPoint = new Vector3(-2.5f, 0.25f, -1.5f);

    private void Awake()
    {
        for(int i = 0; i < maxRat; i++)
        {
            GameObject newRat = Instantiate(ratPrefab, spawnPoint, ratPrefab.transform.rotation);
            rats.Add(newRat);

            Debug.Log(newRat.GetComponent<RatController>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createRat()
    {
        GameObject newRat = Instantiate(ratPrefab, spawnPoint, ratPrefab.transform.rotation);
        
        //newRat.GetComponent<RatController>().patrolPath = 
        rats.Add(newRat);
    }
}
