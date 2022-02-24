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
                    StartCoroutine(boidBehaviour());
                break;
            case (RatState.chasing):
                _ratCatcherInRange();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // collision with ratcatcher
        if (other.name == "Ratcatcher")
            Nest.killRat(this);
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

    // move the boid in accordance to boid behaviour
    IEnumerator boidBehaviour()
    {
        // get the three required vectors
        Vector3 v1, v2, v3;
        v1 = cohesion();
        v2 = seperation();
        v3 = limitVelocity(alignment());

        // get new position
        velocity += v1 + v2 + v3;

        if (!agent.pathPending && agent.remainingDistance < 0.1f)
            agent.SetDestination(transform.position + velocity);


        yield return new WaitForSeconds(1f);
    }

    // rule 1 of boids, try to fly towards center of mass of boids
    Vector3 cohesion()
    {
        // get the average mass from nest
        Vector3 cohesion = Nest.getMass(this);

        // cohesion gives average position
        // only want to move a small way there
        return cohesion / 100;
    }

    // rule 2 of boids, maintain distance from other boids
    Vector3 seperation()
    {
        // might be handled by navmesh, need to see
        return Vector3.zero;
    }

    // rule 3 of boids, match velocity with other boids
    Vector3 alignment()
    {
        // get the average velocity from nest
        Vector3 alignment = Nest.getVelocity(this);

        // add a small amount to the current velocity
        return (alignment - velocity) / 10;
    }

    // limit the velocity to prevent it going too fast
    Vector3 limitVelocity(Vector3 v)
    {
        if (v.magnitude > .001f)
            v = (v / v.magnitude) * baseSpeed;

        return v;
    }
}
