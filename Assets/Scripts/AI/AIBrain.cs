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
    private AIWeaponController _aiWeaponController;
    private PhotonView _PV;
    private Vector3 _startingPos;
    private Vector3 _roamPosition;
    private Transform _chaseTarget;
    private Transform _attackTarget;
    private float _randomRoamTime;
    private float _roamTimer;
    private float _chaseTimer;
    private float _attackTimer;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _aiMovementController = GetComponent<AIMovementController>();
        _aiWeaponController = GetComponent<AIWeaponController>();
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
                    if (_attackTarget != null)
                    {
                        SetState((byte)State.Attack);
                        return;
                    }

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

            case State.Attack:
                {
                    _attackTimer += Time.deltaTime;

                    if (_attackTimer > 2f)
                    {
                        Attack();
                        _attackTimer = 0f;
                    }
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
        _chaseTimer = 0f;
        SetState((byte)State.ResetChase);
    }

    public void SetAttackTarget(Transform target)
    {
        _attackTarget = target;
        _attackTimer = 1.8f;
        SetState((byte)State.Attack);
    }

    public void ResetAttackTarget()
    {
        _attackTarget = null;
        _attackTimer = 0f;
        if (_chaseTarget != null)
        {
            SetState((byte)State.Chase);
        }
        else
        {
            SetState((byte)State.Roam);
        }
    }

    public void ResetAll()
    {
        _chaseTarget = null;
        _attackTarget = null;
        _roamTimer = 0f;
        _chaseTimer = 0f;
        _attackTimer = 0f;
        _randomRoamTime = Random.Range(4.5f, 6f);
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
        _randomRoamTime = Random.Range(4.5f, 6f);
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
        if (_attackTarget == null && Vector3.Distance(transform.position, _roamPosition) < 0.1f)
        {
            _aiMovementController.Move(_roamPosition);
        }
        else
        {
            SetState((byte)State.Roam);
        }
    }

    private void Attack()
    {
        if (_attackTarget != null)
        {
            _aiWeaponController.Attack(_attackTarget);
        }
    }
}
