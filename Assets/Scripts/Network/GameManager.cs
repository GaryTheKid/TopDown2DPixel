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

    // items
    public int maxItemSpawnInWorld;
    public Stack<int> avaliableItemWorldIds;
    public Transform spawnedItemParent;

    // projectiles
    public Transform spawnedProjectileParent;

    private void Awake()
    {
        gameManager = this;
        UpdateRoomPlayerList();
        InitializeItemWorldIdStack();
        spawnedItemParent = GameObject.Find("SpawnedItems").transform;
        spawnedProjectileParent = GameObject.Find("SpawnedProjectiles").transform;
    }

    public void Start()
    {
        SpawnPlayerCharacter();
        ItemWorld.SpawnItemWorld(GameManager.gameManager.itemSpawns[0].position, new Bow(), RequestNewItemWorldId());
        ItemWorld.SpawnItemWorld(GameManager.gameManager.itemSpawns[1].position, new Gun_AK(), RequestNewItemWorldId());
        ItemWorld.SpawnItemWorld(GameManager.gameManager.itemSpawns[2].position, new HealthPotion(3), RequestNewItemWorldId());
        ItemWorld.SpawnItemWorld(GameManager.gameManager.itemSpawns[3].position, new Gun_M4(), RequestNewItemWorldId());
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

    private void InitializeItemWorldIdStack()
    {
        avaliableItemWorldIds = new Stack<int>();
        for (int i = maxItemSpawnInWorld; i > 0; i--)
        {
            avaliableItemWorldIds.Push(i);
        }
    }

    public int RequestNewItemWorldId()
    {
        if (avaliableItemWorldIds.Count > 0)
            return avaliableItemWorldIds.Pop();
        else
        {
            Debug.Log("All Item Ids have been assigned!");
            return -1;
        }
    }

    public void ReleaseItemWorldId(int releasedId)
    {
        avaliableItemWorldIds.Push(releasedId);
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
