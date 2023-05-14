using UnityEngine;
using Photon.Pun;
using Pathfinding;
using System;
using System.Collections.Generic;

public class RPC_AI : MonoBehaviour
{
    public Animator animator;

    private PhotonView _PV;
    private AIMovementController _aiMovementController;
    private AIStatsController _aiStatsController;
    private AIWeaponController _aiWeaponController;
    private AIDestinationSetter _aiDestinationSetter;
    private AIAvatarController _aiAvatarController;
    private AIBrain _aiBrain;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _aiMovementController = GetComponent<AIMovementController>();
        _aiStatsController = GetComponent<AIStatsController>();
        _aiWeaponController = GetComponent<AIWeaponController>();
        _aiDestinationSetter = GetComponent<AIDestinationSetter>();
        _aiAvatarController = GetComponent<AIAvatarController>();
        _aiBrain = GetComponent<AIBrain>();
    }

    /*[PunRPC]
    void RPC_SetAI(byte enemyID)
    {
        // set ai name
        List<string> avatarNames = new List<string>(ItemAssets.itemAssets.enemyAvatarDic.Keys);
        gameObject.name = avatarNames[enemyID] + GetComponent<PhotonView>().ViewID.ToString();

        // set avatar
        for (byte i = 0; i < _aiAvatarController.avatars.Length; i++)
        {
            bool isActive = (i == enemyID);
            var avatar = _aiAvatarController.avatars[i];
            avatar.SetActive(isActive);
            if (isActive)
            {
                _animator = avatar.GetComponent<Animator>();
                _aiMovementController.animator = avatar.GetComponent<Animator>();
                _aiEffectController.SetAIEffectFields(avatar);
            }
        }
    }*/

    [PunRPC]
    void RPC_SetState(byte state)
    {
        _aiBrain.aiState = (AIBrain.State)state; 
    }

    [PunRPC]
    void RPC_Move(Vector2 pos)
    {
        _aiMovementController._moveTarget.position = pos;
    }

    [PunRPC]
    void RPC_Halt()
    {
        //_aiMovementController.entityManager.SetComponentData(_aiMovementController.entity, new PathFollow { pathIndex = -1 });
        _aiMovementController._moveTarget.position = transform.position;
        _aiDestinationSetter.target = _aiMovementController._moveTarget;
        animator.SetBool("isMoving", false);
    }

    [PunRPC]
    void RPC_AIDie()
    {
        _aiStatsController.OnDeath.Invoke();
        _aiStatsController.aiStats.isDead = true;
        _aiMovementController.Stop();
    }

    [PunRPC]
    void RPC_AIRespawn(byte enemyID)
    {
        _aiAvatarController.SetAI(enemyID);
        _aiStatsController.OnRespawn.Invoke();
        _aiStatsController.aiStats.isDead = false;
        _aiMovementController.Respawn();
    }

    [PunRPC]
    void RPC_AIDealDamage(int targetId)
    {
        var target = PhotonView.Find(targetId).transform;
        var enemyPlayer = target.GetComponent<PlayerBuffController>();
        var enemyAI = target.GetComponent<AIBuffController>();

        // player target
        if (enemyPlayer != null)
        {
            enemyPlayer.ReceiveDamage(_PV.ViewID, _aiWeaponController.damageInfo, transform.position);
            animator.SetTrigger("isAttacking");

            // attack towards the enemy
            if (target.transform.position.x >= transform.position.x)
            {
                animator.SetFloat("moveX", 1f);
            }
            else
            {
                animator.SetFloat("moveX", -1f);
            }
        }

        // ai target
        if (enemyAI != null)
        {
            enemyAI.ReceiveDamage(_PV.ViewID, _aiWeaponController.damageInfo, transform.position);
            animator.SetTrigger("isAttacking");

            // attack towards the enemy
            if (target.transform.position.x >= transform.position.x)
            {
                animator.SetFloat("moveX", 1f);
            }
            else
            {
                animator.SetFloat("moveX", -1f);
            }
        }
    }
}