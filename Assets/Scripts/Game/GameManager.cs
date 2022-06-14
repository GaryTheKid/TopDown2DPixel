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
using Utilities;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    // singleton
    public static GameManager gameManager;

    // PV
    public PhotonView PV;

    // spawns
    public List<Transform> playerSpawns;
    public List<Transform> itemSpawns;
    public List<(SpawnArea, int, int)> lootBoxSpawnAreas;
    [Tooltip("This value determines how many objects can be instantiated within 1 spawn area. The larger the more.")]
    public int spawnDensity;
    [Tooltip("This value determines how fast objects can be spawned.")]
    public float spawnWaveTimeStep;
    [Tooltip("This value determines how many objs will be spawned per wave.")]
    public float spawnQuantity;

    // players
    public Player[] playerList;

    // lootBoxes
    [Tooltip("How many loot boxes can exist in the world at the same time.")]
    public short maxLootBoxSpawnInWorld;
    [Tooltip("How long can loot boxes exist.")]
    public float lootBoxWorldLifeTime;
    public Stack<short> avaliableLootBoxWorldIds;

    // items
    [Tooltip("How many items can exist in the world at the same time.")]
    public short maxItemSpawnInWorld;
    public float itemWorldLifeTime;
    public Stack<short> avaliableItemWorldIds;

    // parents
    public Transform lootBoxSpawnAreaParent;
    public Transform spawnedPlayerParent;
    public Transform spawnedLootBoxParent;
    public Transform spawnedItemParent;
    public Transform spawnedProjectileParent;

    // timer
    public float timer;
    public float spawnTimer;

    private void Awake()
    {
        gameManager = this;
        UpdateRoomPlayerList();
        InitializeIdStacks();
        InitializeSpawnAreas();
    }

    public void Start()
    {
        SpawnPlayerCharacter();
        /*if (PhotonNetwork.IsMasterClient)
        {
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
            SpawnLootBoxRandomly();
        }*/
    }

    private void FixedUpdate()
    {
        // must be master client
        if (!PhotonNetwork.IsMasterClient)
            return;

        // running timers
        timer += Time.fixedDeltaTime;
        spawnTimer += Time.fixedDeltaTime;
        if (spawnTimer >= spawnWaveTimeStep)
        {
            // spawn loot boxes randomly
            for (int i = 0; i < spawnQuantity; i++)
            {
                SpawnLootBoxRandomly();
            }
            spawnTimer = 0f;
        }
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

    public void SpawnLootBoxRandomly()
    {
        var randIndex = GetRandomSpawnArea();

        // if run out of spawn space
        if (randIndex == -1)
        {
            return;
        }

        var randSpawnPoint = lootBoxSpawnAreas[randIndex].Item1;
        var spawnPoint = randSpawnPoint.GetRandomPointFromArea();

        // try spawn a loot box spawner
        Transform spawner = Instantiate(ItemAssets.itemAssets.pfLootBoxSpawner, spawnPoint, Quaternion.identity, spawnedLootBoxParent);
        spawner.GetComponent<LootBoxSpawner>().SpawnLootBox(randIndex, lootBoxWorldLifeTime);
    }

    public void SpawnLootBoxRandomlyInArea(int whichArea)
    {
        // check if the selected area is full
        if (lootBoxSpawnAreas[whichArea].Item2 <= lootBoxSpawnAreas[whichArea].Item3)
            return;

        var randSpawnPoint = lootBoxSpawnAreas[whichArea].Item1;
        var spawnPoint = randSpawnPoint.GetRandomPointFromArea();

        // try spawn a loot box spawner
        Transform spawner = Instantiate(ItemAssets.itemAssets.pfLootBoxSpawner, spawnPoint, Quaternion.identity, spawnedLootBoxParent);
        spawner.GetComponent<LootBoxSpawner>().SpawnLootBox(whichArea, lootBoxWorldLifeTime);
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

    private void InitializeSpawnAreas()
    {
        lootBoxSpawnAreas = new List<(SpawnArea, int, int)>();
        foreach (Transform child in lootBoxSpawnAreaParent)
        {
            var area = child.GetComponent<SpawnArea>();

            // calculate max capacity based on density
            int maxCapacity = Mathf.RoundToInt(((area.GetAreaSize()) / 11.25f) * spawnDensity);

            // check override value
            if (area.overrideSpawnDensity != 0)
                maxCapacity = Mathf.RoundToInt(((area.GetAreaSize()) / 11.25f) * area.overrideSpawnDensity);

            lootBoxSpawnAreas.Add((area, maxCapacity, 0));
        }
    }

    private int GetRandomSpawnArea()
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < lootBoxSpawnAreas.Count; i++)
        {
            indices.Add(i);
        }

        // randomly select an available area
        for (int i = 0; i < lootBoxSpawnAreas.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(0, indices.Count);

            // check if the area is full
            if (lootBoxSpawnAreas[randIndex].Item3 < lootBoxSpawnAreas[randIndex].Item2)
            {
                return randIndex;
            }
            else
            {
                indices.Remove(randIndex);
            }
        }

        // if can't find available, return -1
        return -1;
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
