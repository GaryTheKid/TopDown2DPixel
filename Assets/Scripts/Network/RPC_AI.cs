using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPC_AI : MonoBehaviour
{
    private AIMovementController _aiMovementController;
    private AIStatsController _aiStatsController;
    private AIBrain _aiBrain;

    private void Awake()
    {
        _aiMovementController = GetComponent<AIMovementController>();
        _aiStatsController = GetComponent<AIStatsController>();
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
}