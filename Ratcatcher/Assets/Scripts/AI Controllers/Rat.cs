using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Rat : NetworkBehaviour
{
    // attributes
    float baseSpeed = 2f;
    public Vector3 velocity;

    // the navmesh agent
    public UnityEngine.AI.NavMeshAgent agent;

    // nest associated with this rat
    RatNest Nest;

    // a navigator for path finding
    public NavManager navigator;

    // range of rat catcher detection
    float catcherRange = 10f;

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
                if (_ratCatcherInRange())
                {
                    beginChase();
                    break;
                }
                if (!coroutineActive)
                    StartCoroutine(roam());
                break;
            case (RatState.chasing):
                chase();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // collision with ratcatcher
        if (other.name == "Ratcatcher")
        {
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

    bool _ratCatcherInRange()
    {
        // get distance to rat catcher
        float distance = (agent.transform.position - Nest.RatCatcher.transform.position).magnitude;

        // move if in distance
        return (distance < catcherRange);
    }

    void speedOffset()
    {
        // have a 1/10 chance of randomly setting the speed
        if (Random.Range(0, 1) < 0.1)
            changeSpeed(baseSpeed * Random.Range(.5f, 1.5f));
    }

    void pathChange()
    {
        // have a 1/20 chance of randomly setting the speed
        if (Random.Range(0, 1) <= 0.001)
            navigator.moveTo(agent);
    }

    IEnumerator roam()
    {
        coroutineActive = true;
        speedOffset();
        pathChange();
        // if near the end of path, get new destination
        navigator.moveTo(agent);

        yield return new WaitForSeconds(1f);

        coroutineActive = false;
    }

    void beginChase()
    {
        changeSpeed(baseSpeed * 1.5f);
        currState = RatState.chasing;
        agent.SetDestination(Nest.RatCatcher.transform.position);
    }

    void chase()
    {
        if (_ratCatcherInRange())
            agent.SetDestination(Nest.RatCatcher.transform.position);
        else
            currState = RatState.roaming;
    }

}
