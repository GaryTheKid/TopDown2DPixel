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
using UnityEngine.UI;
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

    // connection info
    public Text ping;

    // scoreboard
    public RectTransform scoreboardTemplate;

    // spawns
    [Header("Spawn")]
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
    [Header("Loot Box")]
    [Tooltip("How many loot boxes can exist in the world at the same time.")]
    public short maxLootBoxSpawnInWorld;
    [Tooltip("How long can loot boxes exist.")]
    public float lootBoxWorldLifeTime;

    // items
    [Header("Item")]
    [Tooltip("How many items can exist in the world at the same time.")]
    public short maxItemSpawnInWorld;
    public float itemWorldLifeTime;

    // parents
    [Header("Parents")]
    public Transform lootBoxSpawnAreaParent;
    public Transform spawnedPlayerParent;
    public Transform spawnedLootBoxParent;
    public Transform spawnedItemParent;
    public Transform spawnedProjectileParent;
    public Transform FXParent;
    public Transform scoreboardParent;

    // timer
    [Header("Timer")]
    public float timer;
    public float spawnTimer;

    private void Awake()
    {
        gameManager = this;
        UpdateRoomPlayerList();
        InitializeSpawnAreas();
    }

    public void Start()
    {
        SpawnPlayerCharacter();

        SpawnItem(itemSpawns[1].position, 13, 5);
    }

    private void FixedUpdate()
    {
        // must be master client
        if (!PhotonNetwork.IsMasterClient)
            return;

        // show pin
        ping.text = PhotonNetwork.GetPing().ToString() + "ms";

        // running timers
        timer += Time.fixedDeltaTime;

        // stop spawning loot box if reach max
        if (spawnedLootBoxParent.childCount > maxLootBoxSpawnInWorld)
            return;

        // spawn loot box randomly
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

    #region Character
    private void SpawnPlayerCharacter()
    {
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i].IsLocal)
            {
                var player = PhotonNetwork.Instantiate("Player/PlayerCharacter", playerSpawns[i].position, playerSpawns[i].rotation);
                UpdatePlayerID(player.GetComponent<PhotonView>().ViewID, playerList[i].NickName);

                /*player.name = playerList[i].NickName;
                player.transform.parent = spawnedPlayerParent;
                player.GetComponent<PlayerNetworkController>().playerID = playerList[i].NickName;

                // spawn scoreboard tag
                player.GetComponent<PlayerNetworkController>().scoreboardTag = SpawnScoreboardTag(playerList[i].NickName);*/
            }
            /*else
            {
                // spawn tag for all the other players
                SpawnScoreboardTag(playerList[i].NickName);
            }*/
        }
    }
    #endregion

    #region Room
    public void LeaveRoom()
    {
        PhotonNetwork.Disconnect();
    }

    private void UpdateRoomPlayerList()
    {
        playerList = PhotonNetwork.PlayerList;
    }
    #endregion

    #region Scoreboard
    public void UpdatePlayerID(int viewID, string name)
    {
        Game_Network.UpdatePlayerInfo(PV, viewID, name);
    }

    public void AddScoreUI(string playerID, int score)
    {
        GetPlayerScoreboardTag(playerID).AddScore(score);
    }

    public ScoreboardTag GetPlayerScoreboardTag(string playerID)
    {
        foreach (Transform tag in scoreboardParent)
        {
            var sTag = tag.GetComponent<ScoreboardTag>();
            if (sTag.GetID() == playerID)
            {
                return sTag;
            }
        }

        return null;
    }

    public ScoreboardTag SpawnScoreboardTag(string playerID)
    {
        RectTransform tag = Instantiate(scoreboardTemplate, scoreboardParent);
        tag.name = playerID;
        tag.gameObject.SetActive(true);
        ScoreboardTag scoreboardTag = tag.GetComponent<ScoreboardTag>();
        scoreboardTag.SetID(playerID);
        return scoreboardTag;
    }

    public void DestroyScoreBoardTag(string playerID)
    {
        foreach (Transform tag in scoreboardParent)
        {
            if (tag.GetComponent<ScoreboardTag>().GetID() == playerID)
            {
                Destroy(tag.gameObject);
                break;
            }
        }
    }
    #endregion

    #region Loot Box
    public void SpawnLootBox(Vector2 pos, int areaIndex)
    {
        var newLootBox = PhotonNetwork.InstantiateRoomObject("Item/pfLootBox", pos, Quaternion.identity);
        newLootBox.transform.parent = spawnedLootBoxParent;

        LootBoxWorld lootBoxWorld = newLootBox.GetComponent<LootBoxWorld>();
        lootBoxWorld.SetLootBox(areaIndex);
        lootBoxWorld.Expire();
    }

    public void SpawnLootBoxRandomly()
    {
        var randAreaIndex = GetRandomSpawnArea();

        // if run out of spawn space
        if (randAreaIndex == -1)
        {
            return;
        }

        var randSpawnArea = lootBoxSpawnAreas[randAreaIndex].Item1;
        var spawnPoint = randSpawnArea.GetRandomPointFromArea();

        // try spawn a loot box spawner
        Transform spawner = Instantiate(ItemAssets.itemAssets.pfLootBoxSpawner, spawnPoint, Quaternion.identity, spawnedLootBoxParent);
        spawner.GetComponent<LootBoxSpawner>().SpawnLootBox(randAreaIndex);
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
        spawner.GetComponent<LootBoxSpawner>().SpawnLootBox(whichArea);
    }
    #endregion

    #region Item World
    public ItemWorld SpawnItem(Vector2 pos, short itemID, short amount=1, short durability=100)
    {
        var newItemWorld = PhotonNetwork.InstantiateRoomObject("Item/pfItemWorld", pos, Quaternion.identity);

        ItemWorld itemWorld = newItemWorld.GetComponent<ItemWorld>();
        itemWorld.SetItem(itemID, amount, durability);
        itemWorld.Expire();

        return itemWorld;
    }

    public ItemWorld DropItem(Vector2 pos, short itemID, short amount, short durability)
    {
        var newItemWorld = PhotonNetwork.InstantiateRoomObject("Item/pfItemWorld", pos, Quaternion.identity);

        ItemWorld itemWorld = newItemWorld.GetComponent<ItemWorld>();
        itemWorld.SetItem(itemID, amount, durability);
        itemWorld.Expire();
        itemWorld.AddForce();

        return itemWorld;
    }
    #endregion

    #region Spawn Areas
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
    #endregion

    #region PunCallbacks
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateRoomPlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateRoomPlayerList();
        DestroyScoreBoardTag(otherPlayer.NickName);
    }
    #endregion
}
