using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    // singleton
    public static GameManager gameManager;

    // spawns
    public List<Transform> spawns;

    // players
    public Player[] playerList; 

    private void Awake()
    {
        gameManager = this;
        UpdateRoomPlayerList();
    }

    public void Start()
    {
        SpawnPlayerCharacter();
    }

    private void SpawnPlayerCharacter()
    {
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i].IsLocal)
            {
                PhotonNetwork.Instantiate("Player/PlayerCharacter", spawns[i].position, spawns[i].rotation);
            }
        }
    }

    private void UpdateRoomPlayerList()
    {
        playerList = PhotonNetwork.PlayerList;
    }

    #region Callbacks
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateRoomPlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateRoomPlayerList();
    }
    #endregion
}
