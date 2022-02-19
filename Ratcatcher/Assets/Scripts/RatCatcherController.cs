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

    // time between moving
    public float movementCooldown = 3f;
    public float movementTimer = 3f;

    bool isStunned;

    private void Start()
    {
        agent.speed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(agent.transform.position);
        movementTimer -= Time.deltaTime;

        if(movementTimer < 0.0f && !isStunned)
        {
            playerPosX = Player.transform.position.x;
            playerPosY = Player.transform.position.y;
            playerPosZ = Player.transform.position.z;

            agent.SetDestination(new Vector3(playerPosX, playerPosY, playerPosZ));
            Debug.Log(agent.nextPosition);
        }
        
    }
}
