using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    // attributes
    float baseSpeed = 2f;
    public Vector3 velocity;

    // the navmesh agent
    public UnityEngine.AI.NavMeshAgent agent;

    // nest associated with this rat
    RatNest Nest;

    // is this leader the boids
    //public bool isLeader = false;

    // is there a coroutine running
    bool coroutineActive = false;

    // states for the rats
    public enum RatState
    {
        newborn,   // Newly Made Instance
        roaming,  // Wandering in a set area
        chasing    // Going to Rat Catcher
    }

    RatState currState = RatState.roaming;

    private void Start()
    {
        changeSpeed(baseSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case (RatState.roaming):
                _ratCatcherInRange();
                if (!coroutineActive)
                {
                    coroutineActive = true;
                    StartCoroutine(roam());
                }
                
                break;
            case (RatState.chasing):
                if (!coroutineActive)
                {
                    coroutineActive = true;
                    StartCoroutine(chase());
                }
                break;
            default:
                break;
        }
    }

  /*  private void createMarker(Vector3 v)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = v;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        // collision with ratcatcher
        if (other.name == "Ratcatcher")
        {
            Debug.Log("catch");
            Nest.killRat(this);
        }
            
    }

    private void changeSpeed(float newSpeed)
    {
        agent.speed = newSpeed;
        agent.angularSpeed = newSpeed * 60;
        agent.acceleration = newSpeed * 4;
    }

    public void setNest(RatNest Nest)
    {
        this.Nest = Nest;
    }

    void _ratCatcherInRange()
    {
        // get distance to rat catcher
        float distance = (agent.transform.position - Nest.RatCatcher.transform.position).magnitude;

        // move if in distance
        if (distance < 10)
        {
            currState = RatState.chasing;
            agent.SetDestination(Nest.RatCatcher.transform.position);
        }

    }

    void speedOffset()
    {
        // have a 1/10 chance of randomly setting the speed
        if (Random.Range(0, 1) < 0.1)
            changeSpeed(baseSpeed * Random.Range(.5f, 1.5f));
    }

    IEnumerator roam()
    {
        speedOffset();
        // if near the end of path, get new destination
        if (!agent.pathPending && agent.remainingDistance < 1f)
            agent.SetDestination(Nest.getInstruction());

        yield return new WaitForSeconds(1f);

        coroutineActive = false;
    }

    IEnumerator chase()
    {
        speedOffset();
        // if near the end of path, get new destination
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            agent.SetDestination(Nest.RatCatcher.transform.position);

        yield return new WaitForSeconds(.25f);
        coroutineActive = false;
    }

}
