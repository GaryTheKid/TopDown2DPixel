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
    public List<Transform> playerSpawns;
    public List<Transform> itemSpawns;

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
        ItemWorld.SpawnItemWorld(GameManager.gameManager.itemSpawns[0].position, new Sword(1));
        ItemWorld.SpawnItemWorld(GameManager.gameManager.itemSpawns[1].position, new Axe());
        ItemWorld.SpawnItemWorld(GameManager.gameManager.itemSpawns[2].position, new HealthPotion(3));
        ItemWorld.SpawnItemWorld(GameManager.gameManager.itemSpawns[3].position, new Bow(1));
    }

    private void SpawnPlayerCharacter()
    {
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i].IsLocal)
            {
                PhotonNetwork.Instantiate("Player/PlayerCharacter", playerSpawns[i].position, playerSpawns[i].rotation);
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
