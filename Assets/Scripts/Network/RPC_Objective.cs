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
    }

    [PunRPC]
    void RPC_ResetAndActivateObjective()
    {
        _objective.isActive = true;
        _objective.captureProgress = 0f;
        _objective.capturingPlayer = -1;
    }
}