using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : MonoBehaviour
{
    public Vector3[] patrolPath;

    // states for the rats
    public enum RatState
    {
        newborn,   // Newly Made Instance
        patrolling,  // Wandering in a set area
        chasing    // Going to Rat Catcher
    }

    RatState currState = RatState.newborn;

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case (RatState.newborn):
                break;
            case (RatState.patrolling):
                break;
            case (RatState.chasing):
                break;
            default:
                break;
        }
    }
}
