using UnityEngine;
using System.Collections;
using System;

public class RatCatcherController : MonoBehaviour
{
    const float stunTimerMax = 0.1f;
    const float baseSpeed = 6f;
    const float stunToleranceMax = 5f;
    const int searchRange = 5;
    const int escapeRange = 5;
    private Vector3[] spawnPoints = {
        new Vector3(-10.5f, 1.15f, 11.5f),
        new Vector3(2f, 1.15f, 9.5f),
        new Vector3(5.5f, 1.15f, 0f)
    };

    private int spawnIndex = 0;

    // reference to player (the chased)
    public PlayerMovement Player;

    // the navmesh agent
    public UnityEngine.AI.NavMeshAgent agent;

    // location info of player
    private Vector3 _playerPosition;

    // a set timer
    private float _timer;
    private bool _timerLock;    // true when timer engaged

    private float stunTolerance = stunToleranceMax;
    private float stunTimer = stunTimerMax;
    private float stunChargeRate = stunToleranceMax / 50;
    
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
        changeSpeed(3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_currentState); //debugging to tell which state
        _tick();
        switch (_currentState)
        {
            case (RatCatcherState.inactive):
                if (_setTimer(3f))
                    _changeState(RatCatcherState.searching);
                break;
            case (RatCatcherState.searching):
                _patrol();
                break;
            case (RatCatcherState.chasing):
                _chasePlayer();
                break;
            case (RatCatcherState.stunned):
                bool timerLock = _setTimer(10f);
                stunChargeRate = 0.25f;
                if (timerLock && stunTolerance >= stunToleranceMax)
                    recovered();
                else if (timerLock)
                    _changeState(RatCatcherState.agitated);
                break;
            case (RatCatcherState.agitated):
                _chasePlayer();
                Debug.Log("pissed off");
                Debug.Log(agent.speed);
                break;
            case (RatCatcherState.recovering):
                break;
        }
    }

    /*** STATE AGNOSTIC FUNCTIONS ***/

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
            case (RatCatcherState.chasing):
                agent.isStopped = false;
                break;
            case (RatCatcherState.stunned):
                _currentState = RatCatcherState.stunned;
                agent.isStopped = true;
                agent.ResetPath();
                break;
            case (RatCatcherState.agitated):
                agent.isStopped = false;
                stunChargeRate = 0.1f;
                changeSpeed(baseSpeed * 2);
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
        agent.SetDestination(_getPlayerLocation());

        // check if player has escaped
        if (_playerInRange(escapeRange))
        {
            _changeState(RatCatcherState.searching);
        }
            
    }

    // return the players current position as Vector3
    private Vector3 _getPlayerLocation(){
        return new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
    }

    // return distance to the player
    private bool _playerInRange(float minDist)
    {
        Vector3 currentLocation = _getPlayerLocation();
        Debug.Log("player: " + currentLocation);
        Debug.Log("ratcatcher: " + agent.transform.position);
        float distance = (agent.transform.position - currentLocation).magnitude;
        Debug.Log("distance: " + distance);

        return (distance <= minDist);
    }

    // set a timer, returns true when the timer is done
    private bool _setTimer(float duration)
    {
        if (!_timerLock)
        {
            _timer = duration;
            _timerLock = true;
        }
        else if (_timer < 0f)
            _timerLock = false;

        return !_timerLock;
    }

    // registering a stun hit, called in FlashlightControl.cs stun()
    public void stunHit()
    {
        // used to tell how close to a stun
        stunTolerance -= 1f;

        // transition to stun
        if (stunTolerance < 0f)
            _changeState(RatCatcherState.stunned);
    }

    // ticks all time based variables
    private void _tick()
    {
        float tickLength = Time.deltaTime;

        // decrement timers
        if (_timerLock)
            _timer -= tickLength;
        stunTimer -= tickLength;

        // recharge stun
        if (stunTolerance < stunToleranceMax && stunTimer < 0f)
        {
            stunTolerance += stunChargeRate;
            stunTimer = stunTimerMax;
        }

    }

    /*** INACTIVE STATE FUNCTIONS ***/

    /*** SEARCHING STATE FUNCTIONS ***/

    // cycle spawn points, moving once one close to the player is found
    private void _cycleSpawn() {
        transform.position = spawnPoints[spawnIndex];
        // check that the player is range to be chased
        if (_playerInRange(2)) {
            // move to the correct spawn
            _changeState(RatCatcherState.chasing);    
        } else {
            // move to the next spawn to check
            spawnIndex = (spawnIndex + 1) % spawnPoints.Length;
        }
    }

    // patrol to new destination
    private void _patrol()
    {
        // if player within range, start chasing
        if (_playerInRange(searchRange))
            _changeState(RatCatcherState.chasing);

        // if not already on way to point or close to
        // finishing path, set new destination
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            _setDestination();
        }
    }

    // set destination for a path
    private void _setDestination()
    {
        Debug.Log(spawnPoints[spawnIndex]);
        agent.SetDestination(spawnPoints[spawnIndex]);
        spawnIndex = (spawnIndex + 1) % spawnPoints.Length;
    }

    /*** CHASING STATE FUNCTIONS ***/

    /*** STUNNED STATE FUNCTIONS ***/

    // currently changes state, will do more when recovering state implemented
    private void recovered()
    {
        Debug.Log(spawnIndex);
        stunTolerance = stunToleranceMax;
        stunChargeRate = 0.01f;
        _changeState(RatCatcherState.searching);
    }

    /*** AGITATED STATE FUNCTIONS ***/

    /*** RECOVERING STATE FUNCTIONS ***/
}
