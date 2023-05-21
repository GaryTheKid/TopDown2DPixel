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
        // check if game end
        if (GameManager.singleton.gameState == GameManager.GameState.Ending)
            return;

        // announce who captures this objective 
        string nickName = PhotonNetwork.CurrentRoom.GetPlayer(playerActorNumber).NickName;
        nickName = nickName.Substring(0, nickName.IndexOf("#"));
        GlobalAnnouncementManager.singleton.PlayAnnouncement("Objective has been captured by " + nickName);

        // add score
        foreach (Transform player in GameManager.singleton.spawnedPlayerParent)
        {
            if (player.GetComponent<PhotonView>().OwnerActorNr == playerActorNumber)
            {
                player.GetComponent<PlayerStatsController>().UpdateScore(_objective.captureValue);
                GameManager.singleton.AddScore(playerActorNumber, _objective.captureValue);
            }
        }

        // deactivate
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