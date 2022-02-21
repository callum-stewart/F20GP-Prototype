using UnityEngine;
using System.Collections;

public class RatCatcherController : MonoBehaviour
{
    const float stunTimerMax = 0.1f;

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

    private float stunTolerance = 5f;
    private float stunTimer = stunTimerMax;
    private float stunChargeRate = 0.1f;
    
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
        agent.speed = 6f;
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
                stunChargeRate = 0.5f;
                if (timerLock && stunTolerance >= 5f)
                    recovered();
                else if (timerLock)
                    _changeState(RatCatcherState.agitated);
                break;
            case (RatCatcherState.agitated):
                Debug.Log("pissed off");
                break;
            case (RatCatcherState.recovering):
                break;
        }
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
                stunChargeRate = 0.1f;
                break;
            default:
                break;
        }

        // change state
        _currentState = newState;
    }

    private void _chasePlayer()
    {
        // get player position
        _playerPosition = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);

        agent.SetDestination(_playerPosition);
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

    // when the player successfully stuns the rat catcher
    public void stunned() 
    {
        Debug.Log("StunTol: " + stunTolerance);

 /*       if (_currentStun == -99)
            _currentStun = stunTolerance;

        // monitor for excessive flashing
        if (_currentStun - stunTolerance > 1f)
            _changeState(RatCatcherState.agitated);

        _currentStun = stunTolerance;*/
    }

    // set a timer, returns true when the timer is done
    private bool _setTimer(float duration)
    {
        if (!_timerLock)
        {
            _timer = duration;
            _timerLock = true;
        } else if (_timer < 0f)
            _timerLock = false;

        return !_timerLock;
    }

    // ticks all time based variables
    private void _tick()
    {
        float tickLength = Time.deltaTime;

        // decrement timers
        if(_timerLock)
            _timer -= tickLength;
        movementTimer -= tickLength;
        stunTimer -= tickLength;

        Debug.Log(stunTolerance + " , " + stunTimer);
        // recharge stun
        if (stunTolerance < 5f && stunTimer < 0f)
        {
            Debug.Log(stunTolerance);
            stunTolerance += stunChargeRate;
            stunTimer = stunTimerMax;
        }
            
    }

    // currently changes state, will do more when recovering state implemented
    private void recovered()
    {
        stunTolerance = 5f;
        stunChargeRate = 0.01f;
        _changeState(RatCatcherState.chasing);
    }
}
