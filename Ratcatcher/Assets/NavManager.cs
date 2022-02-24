using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavManager : MonoBehaviour
{
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

    public void moveTo(NavMeshAgent agent)
    {
        // if not already on way to point or close to
        // finishing path, set new destination
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
            setDestination(agent);
    }

    // set destination for a path
    public void setDestination(NavMeshAgent agent)
    {
        Vector3 destination = generateRandomPoint();
        NavMeshPath path = new NavMeshPath();

        // if the path is not blocked, we use it
        if (agent.CalculatePath(destination, path))
        {
            Debug.Log(destination);
            agent.SetDestination(destination);
        }
            
        // path is blocked, find new one
        else
            setDestination(agent);
    }

    public Vector3 generateRandomPoint(bool isSpawn = false)
    {

        int rIndex = isSpawn ? Random.Range(0, points.Length-4)
            : Random.Range(0, points.Length);
        return points[rIndex];
    }
}
