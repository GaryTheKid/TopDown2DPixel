/* Last Edition: 06/10/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The game manager that controls and manages all global game values and functions.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using NetworkCalls;

public class GameManager : MonoBehaviourPunCallbacks
{
    // singleton
    public static GameManager gameManager;

    // PV
    public PhotonView PV;

    // spawns
    public List<Transform> playerSpawns;
    public List<Transform> itemSpawns;

    // players
    public Player[] playerList;

    // lootBoxes
    public short maxLootBoxSpawnInWorld;
    public Stack<short> avaliableLootBoxWorldIds;

    // items
    public short maxItemSpawnInWorld;
    public Stack<short> avaliableItemWorldIds;

    // parents
    public Transform spawnedPlayerParent;
    public Transform spawnedLootBoxParent;
    public Transform spawnedItemParent;
    public Transform spawnedProjectileParent;

    private void Awake()
    {
        gameManager = this;
        UpdateRoomPlayerList();
        InitializeIdStacks();
    }

    public void Start()
    {
        SpawnPlayerCharacter();
        SpawnLootBox(itemSpawns[0].position);
        SpawnItem(itemSpawns[1].position, 2);
        SpawnItem(itemSpawns[2].position, 3);
        SpawnItem(itemSpawns[3].position, 4, 3);
        SpawnItem(itemSpawns[4].position, 5);
        SpawnItem(itemSpawns[5].position, 6);
    }

    private void SpawnPlayerCharacter()
    {
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i].IsLocal)
            {
                var player = PhotonNetwork.Instantiate("Player/PlayerCharacter", playerSpawns[i].position, playerSpawns[i].rotation);
                player.transform.parent = spawnedPlayerParent;
            }
        }
    }

    public void SpawnLootBox(Vector3 pos)
    {
        Game.SpawnLootBox(PV, pos);
    }

    public void SpawnItem(Vector3 pos, short itemId, short amount=1)
    {
        if (amount > 1)
            Game.SpawnItems(PV, pos, itemId, amount);
        else if (amount == 1)
            Game.SpawnItem(PV, pos, itemId);
        else
            Debug.Log("Item amount must be positive");
    }

    private void UpdateRoomPlayerList()
    {
        playerList = PhotonNetwork.PlayerList;
    }

    private void InitializeIdStacks()
    {
        // lootbox stack
        avaliableLootBoxWorldIds = new Stack<short>();
        for (short i = maxLootBoxSpawnInWorld; i > 0; i--)
        {
            avaliableLootBoxWorldIds.Push(i);
        }

        // item stack
        avaliableItemWorldIds = new Stack<short>();
        for (short i = maxItemSpawnInWorld; i > 0; i--)
        {
            avaliableItemWorldIds.Push(i);
        }
    }

    public short RequestNewLootBoxWorldId()
    {
        if (avaliableLootBoxWorldIds.Count > 0)
            return avaliableLootBoxWorldIds.Pop();
        else
        {
            Debug.Log("All Loot Box Ids have been assigned!");
            return -1;
        }
    }

    public void ReleaseLootBoxWorldId(short releasedId)
    {
        avaliableLootBoxWorldIds.Push(releasedId);
    }

    public short RequestNewItemWorldId()
    {
        if (avaliableItemWorldIds.Count > 0)
            return avaliableItemWorldIds.Pop();
        else
        {
            Debug.Log("All Item Ids have been assigned!");
            return -1;
        }
    }

    public void ReleaseItemWorldId(short releasedId)
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
