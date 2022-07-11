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
using Photon.Pun;

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

    public State aiState;

    private AIMovementController _aiMovementController;
    private PhotonView _PV;
    private Vector3 _startingPos;
    private Vector3 _roamPosition;
    private Transform _chaseTarget;
    private float _roamTimer;
    private float _randomRoamTime;
    private float _chaseTimer;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _aiMovementController = GetComponent<AIMovementController>();
    }

    private void Start()
    {
        _startingPos = transform.position;
        _randomRoamTime = Random.Range(5f, 7f);
        SetState((byte)State.Roam);
    }

    private void Update()
    {
        switch (aiState)
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

                    if (_chaseTimer > 1.2f)
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
    public void SetState(byte state)
    {
        _PV.RPC("RPC_SetState", RpcTarget.AllViaServer, state);
    }

    public void SetChaseTarget(Transform target)
    {
        _chaseTarget = target;
        SetState((byte)State.Chase);
    }

    public void ResetChaseTarget()
    {
        _chaseTarget = null;
        SetState((byte)State.ResetChase);
    }

    public void ResetAll()
    {
        _chaseTarget = null;
        _roamTimer = 0f;
        _chaseTimer = 0f;
        _randomRoamTime = Random.Range(5f, 7f);
        SetState((byte)State.Roam);
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
            SetState((byte)State.ResetChase);
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
            SetState((byte)State.Roam);
        }
    }
}
