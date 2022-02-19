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

    public float stunTolerance = 5f;
    public float stunTimer = 0.25f;
    bool isStunned;

    private void Start()
    {
        agent.speed = 12f;
    }

    // Update is called once per frame
    void Update()
    {
        movementTimer -= Time.deltaTime;
        stunTimer -= Time.deltaTime;

        // while not stunned, hunt the player
        if(movementTimer < 0.0f && !isStunned)
        {
            playerPosX = Player.transform.position.x;
            playerPosY = Player.transform.position.y;
            playerPosZ = Player.transform.position.z;

            agent.SetDestination(new Vector3(playerPosX, playerPosY, playerPosZ));
        }

        // recovering between stun hits
        if (stunTolerance < 5f && stunTimer < 0.0f && !isStunned)
        {
            stunTolerance += 0.01f;
            stunTimer = 0.25f;
        }

        // stun duration has ended
        if(isStunned && stunTimer < 0)
        {
                Debug.Log("recovered");
                stunTolerance = 5f;
                isStunned = false;
                agent.isStopped = false;
        }
            

    }

    // registering a stun hit
    public void stunHit()
    {
        stunTolerance -= 1f;

        if (stunTolerance < 0f)
            stunned();
            
    }

    // when the player successfully stuns the rat catcher
    public void stunned() 
    {
        isStunned = true;
        Debug.Log("stunned");
        agent.isStopped = true;
        stunTimer = 3f;
    }
}
