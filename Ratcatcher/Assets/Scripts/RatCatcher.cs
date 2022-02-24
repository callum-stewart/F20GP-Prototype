using UnityEngine;
using System.Collections;
using System;
using UnityEngine.AI;

public class RatCatcher : MonoBehaviour
{
    // reference to player (the chased)
    public PlayerMovement Player;
    // reference to animator
    public Animator Animator;
    // the navmesh agent
    public UnityEngine.AI.NavMeshAgent agent;

    // the nav manager
    NavManager navigator = new NavManager();

    const float stunTimerMax = 0.1f;
    const float baseSpeed = 3f;
    const float stunToleranceMax = 5f;
    const int searchRange = 10;
    const int escapeRange = 15;

    private int spawnIndex = 0;

    // a set timer
    private float _timer;
    private bool _timerLock;    // true when timer engaged

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
                bool timerLock = _setTimer(3f);
                stunChargeRate = 0.25f;
                if (timerLock && stunTolerance >= stunToleranceMax)
                    recovered();
                else if (timerLock)
                    _changeState(RatCatcherState.agitated);
                break;
            case (RatCatcherState.agitated):
                stunTolerance = 100f;
                if (_setTimer(10f))
                    _changeState(RatCatcherState.searching);
                _chasePlayer();
                break;
            case (RatCatcherState.recovering):
                agent.Warp(navigator.generateRandomPoint());
                _changeState(RatCatcherState.inactive);
                break;
        }
    }

    /*** STATE AGNOSTIC FUNCTIONS ***/

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "First Person Player")
            FindObjectOfType<GameManager>().ChangeScene(2);
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
        agent.SetDestination(Player.transform.position);

        // check if player has escaped
        if (!_playerInRange(escapeRange))
            _changeState(RatCatcherState.searching);      
    }

    // return distance to the player
    private bool _playerInRange(float minDist)
    {
        Vector3 currentLocation = Player.transform.position;
        float distance = (agent.transform.position - currentLocation).magnitude;

        return (distance <= minDist);
    }

    private void setAudio(string sound, string oldSound = "")
    {
        AudioManager aM = FindObjectOfType<AudioManager>();

        if (oldSound != "")
            aM.Stop(oldSound);

        aM.Play(sound);
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
        if (stunTolerance < 0f && !stunImmune)
            _changeState(RatCatcherState.stunned);
    }

    // patrol to new destination
    private void _patrol()
    {
        // if player within range, start chasing
        if (_playerInRange(searchRange))
            _changeState(RatCatcherState.chasing);

        navigator.moveTo(agent);
    }

    // currently changes state, will do more when recovering state implemented
    private void recovered()
    {
        stunTolerance = stunToleranceMax;
        stunChargeRate = 0.01f;
        _changeState(RatCatcherState.recovering);
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
}
