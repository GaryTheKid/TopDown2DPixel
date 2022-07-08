/* Last Edition: 07/03/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The AI Brain to control the how AI Behaves.
 * Last Edition:
 *   Just Created.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class AIBrain : MonoBehaviour
{
    public enum State
    {
        Idle,
        Roam,
        Chase,
        ResetChase,
        Attack
    }

    private AIMovementController _aiMovementController;
    private Vector3 _startingPos;
    private Vector3 _roamPosition;
    private Transform _chaseTarget;
    private State _aiState;
    private float _roamTimer;
    private float _randomRoamTime;
    private float _chaseTimer;

    private void Awake()
    {
        _aiMovementController = GetComponent<AIMovementController>();
    }

    private void Start()
    {
        _startingPos = transform.position;
        _randomRoamTime = Random.Range(5f, 7f);
        SetState(State.Roam);
    }

    private void Update()
    {
        switch (_aiState)
        {
            case State.Idle:
                {
                    
                }
                break;

            case State.Roam:
                {
                    _roamTimer += Time.deltaTime;

                    if (_roamTimer > _randomRoamTime)
                    {
                        Roam();
                    }
                }
                break;

            case State.Chase:
                {
                    _chaseTimer += Time.deltaTime;

                    if (_chaseTimer > 1f)
                    {
                        Chase();
                        _chaseTimer = 0f;
                    }
                }
                break;

            case State.ResetChase:
                {
                    ResetChase();
                }
                break;
        }
    }
    public void SetState(State state)
    {
        _aiState = state;
    }

    public void SetChaseTarget(Transform target)
    {
        _chaseTarget = target;
        SetState(State.Chase);
    }

    public void ResetChaseTarget()
    {
        _chaseTarget = null;
        SetState(State.ResetChase);
    }

    private Vector3 GetRoamingPosition()
    {
        return _startingPos + UtilsClass.GetRandomDir() * Random.Range(10f, 15f);
    }

    private void Roam()
    {
        _roamPosition = GetRoamingPosition();
        _aiMovementController.Move(_roamPosition);
        _roamTimer = 0f;
        _randomRoamTime = Random.Range(5f, 7f);
    }

    private void Chase()
    {
        if (_chaseTarget != null)
        {
            _aiMovementController.Halt();
            _aiMovementController.Move(_chaseTarget.position);
        }
        else
        {
            SetState(State.ResetChase);
        }
    }

    private void ResetChase()
    {
        if (Vector3.Distance(transform.position, _roamPosition) < 0.1f)
        {
            _aiMovementController.Move(_roamPosition);
        }
        else
        {
            SetState(State.Roam);
        }
    }
}
