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
using UnityEngine.Rendering.Universal;
using Photon.Pun;
using Photon.Realtime;
using NetworkCalls;
using Utilities;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    // singleton
    public static GameManager gameManager;

    // Network
    [Header("Network")]
    public PhotonView PV;
    public Text ping;

    // scoreboard
    [Header("Scoreboard")]
    public RectTransform scoreboardTemplate;

    // day-night cycle
    public enum DayNight
    {
        Day,
        Night
    }
    [Header("Day-Night Cycle")]
    [Tooltip("The day night enum.")]
    public DayNight dayNight;
    [Tooltip("The global illumination.")]
    public Light2D globalLight;
    [Tooltip("How long a day lasts.")]
    public float dayLength;

    // weather
    public enum Weather 
    {
        Sunny,
        Rainning,
        Snowing,
        Windy,
        Storming,
    }
    [Header("Weather")]
    [Tooltip("The current weather.")]
    public Weather weather;
    [Tooltip("Time gap between 2 weather")]
    public float weatherChangeTimeStep;
    [Tooltip("Time gap variation.")]
    public float weatherChangeTimeStepVariation;
    [Tooltip("How long one weather lasts.")]
    public float weatherLastingTime;
    [Tooltip("Variation of the weather lasting time.")]
    public float weatherLastingVariation;

    // spawns
    [Header("Spawn")]
    public List<Transform> playerSpawns;
    public List<Transform> itemSpawns;
    [Tooltip("(Area, current count, max count)")]
    public List<(SpawnArea, int, int)> lootBoxSpawnAreas;
    [Tooltip("(Area, max count)")]
    public List<(SpawnArea, int)> aiSpawnAreas;
    [Tooltip("This value determines how many objects can be instantiated within 1 spawn area. The larger the more.")]
    public int lootBoxSpawnDensity;
    [Tooltip("This value determines how fast objects can be spawned.")]
    public float lootBoxSpawnWaveTimeStep;
    [Tooltip("This value determines how many objs will be spawned per wave.")]
    public float lootBoxSpawnQuantity;
    [Tooltip("This value determines how many objects can be instantiated within 1 spawn area. The larger the more.")]
    public int aiSpawnDensity;
    [Tooltip("This value determines how fast objects can be spawned.")]
    public float aiSpawnWaveTimeStep;
    [Tooltip("This value determines how many objs will be spawned per wave.")]
    public float aiSpawnQuantity;

    // players
    public Player[] playerList;

    // AIs
    

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
    public Transform aiSpawnAreaParent;
    public Transform spawnedPlayerParent;
    public Transform spawnedAIParent;
    public Transform spawnedLootBoxParent;
    public Transform spawnedItemParent;
    public Transform spawnedProjectileParent;
    public Transform FXParent;
    public Transform scoreboardParent;

    // timer
    [Header("Timer")]
    public float timer;
    public float nextDayNightSwapTime;
    public float nextWeatherStartTime;
    public float nextWeatherLastingTime;
    public float nextWeatherStopTime;
    public float nextAISpawnTime;
    public float nextLootBoxSpawnTime;


    private void Awake()
    {
        gameManager = this;
        UpdateRoomPlayerList();
        InitializeSpawnAreas();
    }

    public void Start()
    {
        SpawnPlayerCharacter();

        // Debug Items
        SpawnItem(itemSpawns[1].position, 19, 1, 99);

        if (!PhotonNetwork.IsMasterClient)
            return;

        InitializeTimer();
    }

    private void Update()
    {
        // must be master client
        if (!PhotonNetwork.IsMasterClient)
            return;

        // show pin
        ping.text = PhotonNetwork.GetPing().ToString() + "ms";

        // timer
        timer += Time.deltaTime;

        // day-night cycle
        if (timer >= nextDayNightSwapTime)
        {
            switch (dayNight)
            {
                case DayNight.Day:
                    DayToNight();
                    dayNight = DayNight.Night;
                    break;
                case DayNight.Night:
                    NightToDay();
                    dayNight = DayNight.Day;
                    break;
            }
            nextDayNightSwapTime += dayLength;
        }

        // weather
        if (timer >= nextWeatherStartTime)
        {
            StartRandomWeather();
            nextWeatherStartTime += nextWeatherLastingTime + weatherChangeTimeStep + UnityEngine.Random.Range(0f, weatherChangeTimeStepVariation);
        }

        // reset weather
        if(timer >= nextWeatherStopTime)
        {
            ChangeWeather(0);
            nextWeatherLastingTime = weatherLastingTime + UnityEngine.Random.Range(0f, weatherLastingVariation);
            nextWeatherStopTime = nextWeatherStartTime + nextWeatherLastingTime;
        }

        // spawn ai randomly
        if (timer >= nextAISpawnTime)
        {
            // spawn loot boxes randomly
            for (int i = 0; i < aiSpawnQuantity; i++)
            {
                SpawnAIRandomly();
            }
            nextAISpawnTime += aiSpawnWaveTimeStep;
        }

        // spawn loot box randomly
        if (!ObjectPool.objectPool.isAllLootBoxActive && timer >= nextLootBoxSpawnTime)
        {
            // spawn loot boxes randomly
            for (int i = 0; i < lootBoxSpawnQuantity; i++)
            {
                SpawnLootBoxRandomly();
            }
            nextLootBoxSpawnTime += lootBoxSpawnWaveTimeStep;
        }
    }

    #region Timer
    private void InitializeTimer()
    {
        timer = 0f;
        nextDayNightSwapTime = dayLength;
        nextWeatherStartTime = weatherChangeTimeStep + UnityEngine.Random.Range(0f, weatherChangeTimeStepVariation);
        nextWeatherLastingTime = weatherLastingTime + UnityEngine.Random.Range(0f, weatherLastingVariation);
        nextWeatherStopTime = nextWeatherStartTime + nextWeatherLastingTime;
        nextAISpawnTime = aiSpawnWaveTimeStep;
        nextLootBoxSpawnTime = lootBoxSpawnWaveTimeStep;
    }
    #endregion

    #region Day-Night Cycle
    private void NightToDay()
    {
        Game_Network.NightToDay(PV);
    }

    private void DayToNight()
    {
        Game_Network.DayToNight(PV);
    }
    #endregion

    #region Weather
    public void StartRandomWeather()
    {
        var randWeatherCode = (byte)UnityEngine.Random.Range(0, 1 + 1);
        ChangeWeather(randWeatherCode);
    }

    public void ChangeWeather(byte weatherCode)
    {
        Game_Network.ChangeWeather(PV, weatherCode);
    }
    #endregion

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
    public void SpawnLootBox(Vector2 pos, int areaIndex, out bool succeed)
    {
        int requestedLootBoxIndex = ObjectPool.objectPool.RequestLootBoxIndexFromPool();
        if (requestedLootBoxIndex != -1)
        {
            Game_Network.SpawnLootBox(PV, requestedLootBoxIndex, pos);
            LootBoxWorld lootBoxWorld = ObjectPool.objectPool.pooledLootBoxes[requestedLootBoxIndex].GetComponent<LootBoxWorld>();
            lootBoxWorld.SetLootBox(areaIndex);
            lootBoxWorld.Expire();
            succeed = true;
        }
        else
        {
            succeed = false;
        }
    }

    public void SpawnLootBoxRandomly()
    {
        var randAreaIndex = GetRandomSpawnArea_LootBox();

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

    #region AI
    public void SpawnAI(Vector2 pos)
    {
        int requestedAIIndex = ObjectPool.objectPool.RequestAIIndexFromPool();
        if (requestedAIIndex != -1)
        {
            Game_Network.SpawnAI(PV, requestedAIIndex, pos);
            AIWorld aiWorld = ObjectPool.objectPool.pooledAI[requestedAIIndex].GetComponent<AIWorld>();
            aiWorld.SetAI();
        }
    }

    public void SpawnAIRandomly()
    {
        var randAreaIndex = GetRandomSpawnArea_AI();

        // if run out of spawn space
        if (randAreaIndex == -1)
        {
            return;
        }

        var randSpawnArea = aiSpawnAreas[randAreaIndex].Item1;
        var spawnPoint = randSpawnArea.GetRandomPointFromArea();

        // try spawn an AI spawner
        Transform spawner = Instantiate(ItemAssets.itemAssets.pfAISpawner, spawnPoint, Quaternion.identity, spawnedAIParent);
        spawner.GetComponent<AISpawner>().SpawnAI(randAreaIndex);
    }

    public void SpawnAIRandomlyInArea(int whichArea)
    {
        var randSpawnPoint = aiSpawnAreas[whichArea].Item1;
        var spawnPoint = randSpawnPoint.GetRandomPointFromArea();

        // try spawn an AI spawner
        Transform spawner = Instantiate(ItemAssets.itemAssets.pfAISpawner, spawnPoint, Quaternion.identity, spawnedAIParent);
        spawner.GetComponent<AISpawner>().SpawnAI(whichArea);
    }
    #endregion

    #region Item World
    public void SpawnItem(Vector2 pos, short itemID, short amount=1, short durability=100)
    {
        /*var newItemWorld = PhotonNetwork.InstantiateRoomObject("Item/pfItemWorld", pos, Quaternion.identity);

        ItemWorld itemWorld = newItemWorld.GetComponent<ItemWorld>();
        itemWorld.SetItem(itemID, amount, durability);
        itemWorld.Expire();*/

        int requestedItemWorldIndex = ObjectPool.objectPool.RequestItemWorldIndexFromPool();

        if (requestedItemWorldIndex != -1)
        {
            Game_Network.SpawnItemWorld(PV, requestedItemWorldIndex, pos, itemID, amount, durability);
        }

        //return ObjectPool.objectPool.pooledItemWorld[requestedItemWorldIndex];
    }

    public void DropItem(Vector2 pos, short itemID, short amount, short durability)
    {
        int requestedItemWorldIndex = ObjectPool.objectPool.RequestItemWorldIndexFromPool();

        if (requestedItemWorldIndex != -1)
        {
            Game_Network.DropItemWorld(PV, requestedItemWorldIndex, Utilities.Math.GetRandomDegree(), pos, itemID, amount, durability);
        }

        /*var newItemWorld = PhotonNetwork.InstantiateRoomObject("Item/pfItemWorld", pos, Quaternion.identity);

        ItemWorld itemWorld = newItemWorld.GetComponent<ItemWorld>();
        itemWorld.SetItem(itemID, amount, durability);
        itemWorld.Expire();
        itemWorld.AddForce();

        return itemWorld;*/
    }
    #endregion

    #region Spawn Areas
    private void InitializeSpawnAreas()
    {
        // loot box
        lootBoxSpawnAreas = new List<(SpawnArea, int, int)>();
        foreach (Transform child in lootBoxSpawnAreaParent)
        {
            var area = child.GetComponent<SpawnArea>();

            // calculate max capacity based on density
            int maxCapacity = Mathf.RoundToInt(((area.GetAreaSize()) / 11.25f) * lootBoxSpawnDensity);

            // check override value
            if (area.overrideSpawnDensity != 0)
                maxCapacity = Mathf.RoundToInt(((area.GetAreaSize()) / 11.25f) * area.overrideSpawnDensity);

            lootBoxSpawnAreas.Add((area, maxCapacity, 0));
        }

        // ai
        aiSpawnAreas = new List<(SpawnArea, int)>();
        foreach (Transform child in aiSpawnAreaParent)
        {
            var area = child.GetComponent<SpawnArea>();

            // calculate max capacity based on density
            int maxCapacity = Mathf.RoundToInt(((area.GetAreaSize()) / 11.25f) * aiSpawnDensity);

            // check override value
            if (area.overrideSpawnDensity != 0)
                maxCapacity = Mathf.RoundToInt(((area.GetAreaSize()) / 11.25f) * area.overrideSpawnDensity);

            aiSpawnAreas.Add((area, maxCapacity));
        }
    }

    private int GetRandomSpawnArea_LootBox()
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

    private int GetRandomSpawnArea_AI()
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < aiSpawnAreas.Count; i++)
        {
            indices.Add(i);
        }

        // randomly select an available area
        for (int i = 0; i < aiSpawnAreas.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(0, indices.Count);

            // check if the area is full
            var area = aiSpawnAreas[randIndex].Item1;
            Collider2D[] hitCollider = Physics2D.OverlapAreaAll(area.GetV1(), area.GetV2(), LayerMask.NameToLayer("EnemyAI"));
            if (hitCollider != null && hitCollider.Length <= aiSpawnAreas[randIndex].Item2)
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
