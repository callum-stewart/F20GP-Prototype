using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    // attributes
    float baseSpeed = 2f;
    Vector3 Velocity;

    // the navmesh agent
    public UnityEngine.AI.NavMeshAgent agent;

    // nest associated with this rat
    RatNest Nest;

    // is this leader the boids
    public bool isLeader = false;

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
            case (RatState.newborn):
                break;
            case (RatState.roaming):
                if (_ratCatcherInRange())
                    currState = RatState.chasing;
                else if (isLeader)
                    StartCoroutine(roam());
                else
                    StartCoroutine(follow());
                break;
            case (RatState.chasing):
                _ratCatcherInRange();
                break;
            default:
                break;
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
        float distance = (agent.transform.position - Nest.RatCatcher.transform.position).magnitude;
        if (distance < 10)
            return agent.SetDestination(Nest.RatCatcher.transform.position);
        else
            return false;
    }

    Vector3 ratCatcherPosition()
    {
        return Nest.RatCatcher.transform.position;
    }

    IEnumerator roam()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
            agent.SetDestination(Nest.getInstruction(isLeader));

        yield return new WaitForSeconds(1f);
    }

    IEnumerator follow()
    {
        agent.SetDestination(Nest.getInstruction(isLeader));
        yield return new WaitForSeconds(.1f);
    }

    // rule 1 of boids, try to fly towards center of mass of boids
    void cohesion()
    {

    }
}
