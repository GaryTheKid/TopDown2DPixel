using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPC_AI : MonoBehaviour
{
    private PhotonView _PV;
    private AIMovementController _aiMovementController;
    private AIStatsController _aiStatsController;
    private AIWeaponController _aiWeaponController;
    private AIBrain _aiBrain;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _aiMovementController = GetComponent<AIMovementController>();
        _aiStatsController = GetComponent<AIStatsController>();
        _aiWeaponController = GetComponent<AIWeaponController>();
        _aiBrain = GetComponent<AIBrain>();
    }

    [PunRPC]
    void RPC_SetAI()
    {
        gameObject.name = "EnemyAI_Dummy" + GetComponent<PhotonView>().ViewID.ToString();
    }

    [PunRPC]
    void RPC_SetState(byte state)
    {
        _aiBrain.aiState = (AIBrain.State)state; 
    }

    [PunRPC]
    void RPC_Move(Vector2 pos)
    {
        _aiMovementController.MoveTo(pos);
    }

    [PunRPC]
    void RPC_Halt()
    {
        _aiMovementController.entityManager.SetComponentData(_aiMovementController.entity, new PathFollow { pathIndex = -1 });
    }

    [PunRPC]
    void RPC_AIDie()
    {
        _aiMovementController.Stop();
        _aiStatsController.OnDeath.Invoke();
        _aiStatsController.aiStats.isDead = true;
    }

    [PunRPC]
    void RPC_AIRespawn()
    {
        _aiStatsController.OnRespawn.Invoke();
        _aiStatsController.aiStats.isDead = false;
    }

    [PunRPC]
    void RPC_AIDealDamage(int targetId)
    {
        var target = PhotonView.Find(targetId).transform;
        var enemyPlayer = target.GetComponent<PlayerBuffController>();
        var enemyAI = target.GetComponent<AIBuffController>();

        if (enemyPlayer != null)
        {
            enemyPlayer.ReceiveDamage(_PV.ViewID, _aiWeaponController.damageInfo, transform.position);
        }

        if (enemyAI != null)
        {
            enemyAI.ReceiveDamage(_PV.ViewID, _aiWeaponController.damageInfo, transform.position);
        }
    }
}