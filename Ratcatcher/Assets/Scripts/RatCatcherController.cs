using UnityEngine;
using System.Collections;

public class RatCatcherController : MonoBehaviour
{
    const float stunTimerMax = 0.1f;
    const float baseSpeed = 6f;
    const float stunToleranceMax = 5f;

    // reference to player (the chased)
    public PlayerMovement Player;

    // the navmesh agent
    public UnityEngine.AI.NavMeshAgent agent;

    // location info of player
    private Vector3 _playerPosition;

    // time between moving
    private float movementCooldown = 3f;
    private float movementTimer = 3f;

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
        //Debug.Log(_currentState); //debugging to tell which state
        _tick();
        switch (_currentState)
        {
            case (RatCatcherState.inactive):
                if (_setTimer(3f))
                    _changeState(RatCatcherState.chasing);
                break;
            case (RatCatcherState.searching):
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
        _playerPosition = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);

        agent.SetDestination(_playerPosition);
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
        movementTimer -= tickLength;
        stunTimer -= tickLength;

        Debug.Log(stunTolerance + " , " + stunTimer);
        // recharge stun
        if (stunTolerance < stunToleranceMax && stunTimer < 0f)
        {
            Debug.Log(stunTolerance);
            stunTolerance += stunChargeRate;
            stunTimer = stunTimerMax;
        }

    }

    /*** INACTIVE STATE FUNCTIONS ***/

    /*** SEARCHING STATE FUNCTIONS ***/

    /*** CHASING STATE FUNCTIONS ***/

    /*** STUNNED STATE FUNCTIONS ***/

    // currently changes state, will do more when recovering state implemented
    private void recovered()
    {
        stunTolerance = stunToleranceMax;
        stunChargeRate = 0.01f;
        _changeState(RatCatcherState.chasing);
    }

    /*** AGITATED STATE FUNCTIONS ***/

    /*** RECOVERING STATE FUNCTIONS ***/
}
