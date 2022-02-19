using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatCatcherController : MonoBehaviour
{
    // reference to player (the chased)
    public PlayerMovement Player;

    // the navmesh agent
    public UnityEngine.AI.NavMeshAgent agent;

    // location info of player
    private float playerPosX, playerPosY, playerPosZ;

    // Update is called once per frame
    void Update()
    {
        playerPosX = Player.transform.position.x;
        playerPosY = Player.transform.position.y;
        playerPosZ = Player.transform.position.z;

        agent.SetDestination(new Vector3(playerPosX, playerPosY, playerPosZ));
    }
}
