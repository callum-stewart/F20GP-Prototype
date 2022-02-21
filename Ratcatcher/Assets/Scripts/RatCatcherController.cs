using UnityEngine;
using System.Collections;

public class RatCatcherController : MonoBehaviour
{
    // reference to player (the chased)
    public PlayerMovement Player;

    // the navmesh agent
    public UnityEngine.AI.NavMeshAgent agent;

    // location info of player
    private Vector3 _playerPosition;

    // time between moving
    public float movementCooldown = 3f;
    public float movementTimer = 3f;

    // a set timer
    private float _timer;
    private bool _timerLock;    // true when timer engaged

    public float stunTolerance = 5f;
    public float stunTimer = 0.25f;
    
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
                stunned();
                if (_setTimer(3f))
                    recovered();
                break;
            case (RatCatcherState.agitated):
                break;
            case (RatCatcherState.recovering):
                break;
        }
    }

    private void _changeState(RatCatcherState newState)
    {
        switch (newState)
        {
            case (RatCatcherState.stunned):
                _currentState = RatCatcherState.stunned;
                stunTimer = 3f;
                agent.isStopped = true;
                break;
            default:
                break;
        }

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
        Debug.Log("stun hit: " + stunTolerance);
        // used to tell how close to a stun
        stunTolerance -= 1f;

        // transition to stun
        if (stunTolerance < 0f)
            _changeState(RatCatcherState.stunned);
    }

    // when the player successfully stuns the rat catcher
    public void stunned() 
    {
        float currentStun = stunTolerance;

        // monitor for excessive flashing
        if (currentStun - stunTolerance > 3f)
            _changeState(RatCatcherState.agitated);
        
    }

    // set a timer, returns true when the timer is done
    private bool _setTimer(float duration)
    {
        Debug.Log(_timer);

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

        // recharge stun
        if(stunTolerance < 5f && stunTimer < 0f)
        {
            stunTolerance += 0.01f;
            stunTimer = 0.25f;
        }
            
    }

    // currently changes state, will do more when recovering state implemented
    private void recovered()
    {
        _changeState(RatCatcherState.chasing);
        agent.isStopped = false;
    }
}
