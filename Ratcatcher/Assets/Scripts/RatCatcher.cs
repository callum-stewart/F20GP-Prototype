using UnityEngine;
using System.Collections;
using System;
using UnityEngine.AI;
using Mirror;

public class RatCatcher : NetworkBehaviour
{
    // list of players (the chased)
    public GameObject[] Players;
    // reference to animator
    public Animator Animator;
    // the navmesh agent
    public UnityEngine.AI.NavMeshAgent agent;

    // the nav manager
    NavManager navigator = new NavManager();

    const float stunTimerMax = 0.1f;
    const float baseSpeed = 3f;
    const float stunToleranceMax = 3f;
    const int searchRange = 10;
    const int escapeRange = 15;

    private int spawnIndex = 0;

    // a set timer
    private bool coroutineActive = false;

    private float stunTolerance = stunToleranceMax;
    private float stunTimer = stunTimerMax;
    private float stunChargeRate = stunToleranceMax / 50;
    private bool stunImmune = false;
    
    // states for the enemy
    public enum RatCatcherState {
        inactive,   // before the player releases them
        searching,  // while searching for player
        chasing,    // found player, now persuing
        stunned,    // when suffering from a flashlight stun
        agitated,   // when 
        recovering
    }
    private RatCatcherState _currentState = RatCatcherState.inactive;

    private void Start()
    {
        changeSpeed(baseSpeed);
        Animator.Play("Z_idle", 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_currentState); //debugging to tell which state
        _tick();
        switch (_currentState)
        {
            case (RatCatcherState.inactive):
                // do nothing
                break;
            case (RatCatcherState.searching):
                _patrol();
                break;
            case (RatCatcherState.chasing):
                _chasePlayer();
                break;
            case (RatCatcherState.stunned):
                if (!coroutineActive)
                    StartCoroutine(checkAgitated());
                break;
            case (RatCatcherState.agitated):
                stunTolerance = 100f;
                if (!coroutineActive)
                    StartCoroutine(isAgitated());
                _chasePlayer();
                break;
            case (RatCatcherState.recovering):
                agent.Warp(navigator.generateRandomPoint());
                _changeState(RatCatcherState.searching);
                break;
        }
    }

    /*** STATE AGNOSTIC FUNCTIONS ***/

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            FindObjectOfType<GameManager>().ChangeScene(2);
    }

    IEnumerator checkAgitated()
    {
        coroutineActive = true;
        stunChargeRate = 0.25f;
        yield return new WaitForSeconds(3);

        if (stunTolerance < stunToleranceMax)
            _changeState(RatCatcherState.agitated);
        else
            _changeState(RatCatcherState.recovering);

        stunChargeRate = 0.01f;
        coroutineActive = false;
    }

    IEnumerator isAgitated()
    {
        coroutineActive = true;
        yield return new WaitForSeconds(10);
        _changeState(RatCatcherState.searching);
        coroutineActive = false;
    }

    public void Activate()
    {
        _changeState(RatCatcherState.searching);
    }

    private void changeSpeed(float newSpeed)
    {
        agent.speed = newSpeed;
        agent.angularSpeed = newSpeed * 60;
        agent.acceleration = newSpeed * 4;
    }

    private void _changeState(RatCatcherState newState)
    {
        // prepare for new state
        switch (newState)
        {
            case (RatCatcherState.searching):
                setAudio("Ambience", "Chase Music");
                Animator.SetTrigger("searching");
                changeSpeed(baseSpeed);
                break;
            case (RatCatcherState.chasing):
                setAudio("Chase Music", "Ambience");
                changeSpeed(baseSpeed * 2);
                agent.isStopped = false;
                break;
            case (RatCatcherState.stunned):
                Animator.SetTrigger("stunned");
                _currentState = RatCatcherState.stunned;
                agent.isStopped = true;
                agent.ResetPath();
                break;
            case (RatCatcherState.agitated):
                Animator.SetTrigger("searching");
                agent.isStopped = false;
                changeSpeed(baseSpeed * 2);
                break;
            case (RatCatcherState.recovering):
                stunTolerance = stunToleranceMax;
                break;
            default:
                break;
        }
        // change state
        _currentState = newState;
    }

    // moves in direction of the player
    private void _chasePlayer()
    {
        // get player position
        agent.SetDestination(FindNearestPlayer().transform.position);

        // check if player has escaped
        if (!_playerInRange(escapeRange, FindNearestPlayer()))
            _changeState(RatCatcherState.searching);      
    }

    // return distance to the player
    private bool _playerInRange(float minDist, GameObject Player)
    {

        Vector3 currentLocation = Player.transform.position;
        float distance = (agent.transform.position - currentLocation).magnitude;

        return (distance <= minDist);
    }

    private void FindPlayers()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
    }

    private GameObject FindNearestPlayer()
    {
        FindPlayers();
        GameObject nearest = Players[0];
        foreach (GameObject player in Players)
        {
            if (nearest == null)
                nearest = player;
            else
                if ((agent.transform.position - player.transform.position).magnitude < (agent.transform.position - nearest.transform.position).magnitude)
                nearest = player;
        }
        return nearest;
    }

    private void setAudio(string sound, string oldSound = "")
    {
        AudioManager aM = FindObjectOfType<AudioManager>();

        if (oldSound != "")
            aM.Stop(oldSound);

        aM.Play(sound);
    }

    // registering a stun hit, called in FlashlightControl.cs stun()
    public void stunHit()
    {
        // used to tell how close to a stun
        stunTolerance -= 1f;

        // transition to stun
        if (stunTolerance < 0f && !stunImmune)
            _changeState(RatCatcherState.stunned);
    }

    // patrol to new destination
    private void _patrol()
    {
        // if player within range, start chasing
        if (_playerInRange(searchRange, FindNearestPlayer()))
            _changeState(RatCatcherState.chasing);

        navigator.moveTo(agent);
    }

    // ticks all time based variables
    private void _tick()
    {
        float tickLength = Time.deltaTime;
        stunTimer -= tickLength;

        // recharge stun
        if (stunTolerance < stunToleranceMax && stunTimer < 0f)
        {
            stunTolerance += stunChargeRate;
            stunTimer = stunTimerMax;
        }

    }
}
