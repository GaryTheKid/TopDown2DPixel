using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPC_Objective : MonoBehaviour
{
    private Objective _objective;

    private void Awake()
    {
        _objective = GetComponent<Objective>();
    }

    [PunRPC]
    void RPC_SendObjectiveCaptureMessage(byte playerActorNumber)
    {
        print("Point captured by: " + playerActorNumber);
        _objective.isActive = false;
        foreach (var obj in GameManager.singleton.objectiveList)
        {
            if (obj.isActive)
            {
                GameManager.singleton.isAnyObjectiveActive = true;
                return;
            }
        }
        GameManager.singleton.isAnyObjectiveActive = false;
    }

    [PunRPC]
    void RPC_ResetAndActivateObjective()
    {
        _objective.isActive = true;
        GameManager.singleton.isAnyObjectiveActive = true;
        _objective.captureProgress = 0f;
        _objective.capturingPlayer = -1;
    }
}