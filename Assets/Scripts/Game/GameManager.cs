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
    // const 
    private const float SPAWN_TIME_STEP = 0.5f;

    // singleton
    public static GameManager singleton;

    // Network
    [Header("Network")]
    public PhotonView PV;
    public Text ping;

    // Level
    [Header("Level")]
    public string levelName;
    [Range(0f, 5f), Header("Preparation Time")]
    public float preparationTime;

    // scoreboard
    [Header("Scoreboard")]
    public RectTransform scoreboardTemplate;

    // game states
    public enum GameState
    {
        Starting,
        Playing,
        Ending
    }
    public GameState gameState;

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
        Windy_East,
        Windy_West,
        Windy_North,
        Windy_South,
        Snowing,
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
    [Tooltip("(Area, current count, max count)")]
    public List<(SpawnArea, int, int)> merchantSpawnAreas;
    [Tooltip("(Area, max count)")]
    public List<(SpawnArea, int)> aiSpawnAreas;
    [Tooltip("This value determines how many objects can be instantiated within 1 spawn area. The larger the more.")]
    public int lootBoxSpawnDensity;
    [Tooltip("This value determines how fast objects can be spawned.")]
    public float lootBoxSpawnWaveTimeStep;
    [Tooltip("This value determines how many objs will be spawned per wave.")]
    public float lootBoxSpawnQuantity;
    [Tooltip("This value determines how many objects can be instantiated within 1 spawn area. The larger the more.")]
    public int merchantSpawnDensity;
    [Tooltip("This value determines how fast objects can be spawned.")]
    public float merchantSpawnWaveTimeStep;
    [Tooltip("This value determines how many objs will be spawned per wave.")]
    public float merchantSpawnQuantity;
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
    [Tooltip("How long loot boxes exist.")]
    public float lootBoxWorldLifeTime;

    // items
    [Header("Item")]
    [Tooltip("How many items can exist in the world at the same time.")]
    public short maxItemSpawnInWorld;
    public float itemWorldLifeTime;

    // merchant
    [Header("Merchant")]
    [Tooltip("How many merchants can exist in the world at the same time.")]
    public short maxMerchantSpawnInWorld;
    [Tooltip("How long merchants exist.")]
    public float merchantLifeTime;

    // objective list
    [Header("Objective")]
    [Tooltip("The objectives for players to gain scores.")]
    public Objective[] objectiveList;
    [Tooltip("Variation of the weather lasting time.")]
    public float objectiveAnnouncementTimeBeforeActivation;
    [Tooltip("This value determines the time between two waves of objectives are spawned.")]
    public float objectiveActivationTimeStep;
    [Tooltip("Is any objective active")]
    public bool isAnyObjectiveActive;

    // parents
    [Header("Parents")]
    public Transform lootBoxSpawnAreaParent;
    public Transform aiSpawnAreaParent;
    public Transform merchantSpawnAreaParent;
    public Transform spawnedPlayerParent;
    public Transform spawnedAIParent;
    public Transform spawnedLootBoxParent;
    public Transform spawnedItemParent;
    public Transform spawnedMerchantParent;
    public Transform spawnedProjectileParent;
    public Transform spawnedDeployableParent;
    public Transform objectiveParent;
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
    public float nextMerchantSpawnTime;
    public float nextObjectiveActivationTime;
    public float nextObjectiveActivationAnnouncementTime;

    private void Awake()
    {
        singleton = this;
        UpdateRoomPlayerList();
        InitializeSpawnAreas();
        InitializeObjectiveList();
        gameState = GameState.Starting;
    }

    public void Start()
    {
        SpawnPlayerCharacter();

        // show level name
        GlobalAnnouncementManager.singleton.PlayAnnouncement(levelName);

        // suspend till the announcement finished, then switch to game state Playing
        SwitchGameStateAfterSuspension(preparationTime, (byte)GameState.Playing);

        // Debug Items
        //SpawnItem(itemSpawns[1].position, 10, 1, 99);
        //SpawnItem(itemSpawns[1].position, 6, 1, 99);
        //SpawnItem(itemSpawns[1].position, 2, 1, 99);
        //SpawnItem(itemSpawns[1].position, 19, 1, 99);
        //SpawnItem(itemSpawns[1].position, 11, 1, 99);
        //SpawnItem(itemSpawns[1].position, 21, 10);
        //SpawnItem(itemSpawns[1].position, 16, 1, 99);

        if (!PhotonNetwork.IsMasterClient)
            return;

        InitializeTimer();
    }

    private void Update()
    {
        // show pin
        ping.text = PhotonNetwork.GetPing().ToString() + "ms";

        // must be master client
        if (!PhotonNetwork.IsMasterClient)
            return;

        // timer
        timer += Time.deltaTime;

        switch (gameState)
        {
            case GameState.Starting:
                {
                    break;
                }

            case GameState.Playing:
                {
                    // objective
                    HandleObjectiveActivation();

                    // day-night cycle
                    HandleDayNightCircle();

                    // weather
                    HandleWeatherChange();

                    // AI
                    AISpawning();

                    // lootbox
                    LootboxSpawning();

                    // merchant
                    MerchantSpawning();

                    break;
                }

            case GameState.Ending:
                {
                    break;
                }
        }
    }

    #region Game State
    public void SwitchGameStateAfterSuspension(float time, byte newState)
    {
        StartCoroutine(Co_SwitchGameStateAfterSuspension(time, newState));
    }
    IEnumerator Co_SwitchGameStateAfterSuspension(float time, byte newState)
    {
        yield return new WaitForSecondsRealtime(time);
        SwitchGameState(newState);
    }

    public void SwitchGameState(byte newState)
    {
        Game_Network.SwitchGameState(PV, newState);
    }
    #endregion

    #region Game Event Handling
    private void HandleObjectiveActivation()
    {
        // make an announcement in advance
        if (timer >= nextObjectiveActivationAnnouncementTime)
        {
            AnnounceIncommingObjectiveActivationEvent();
            nextObjectiveActivationAnnouncementTime += objectiveActivationTimeStep;
        }

        // activate objectives
        if (timer >= nextObjectiveActivationTime)
        {
            AnnounceObjectivesHaveBeenActivated();
            ActivateObjectivesRandomly();
            nextObjectiveActivationTime += objectiveActivationTimeStep;
        }
    }
    private IEnumerator Co_ObjectiveActivationHandling()
    {
        while (gameState == GameState.Playing)
        {
            // wait if there is any objective currently active
            yield return new WaitWhile(() => { return isAnyObjectiveActive; });

            // wait for the announcement
            yield return new WaitForSecondsRealtime(objectiveAnnouncementTimeBeforeActivation);
            AnnounceIncommingObjectiveActivationEvent();

            // wait for the time step for activation
            yield return new WaitForSecondsRealtime(objectiveActivationTimeStep);
            AnnounceObjectivesHaveBeenActivated();
            ActivateObjectivesRandomly();
        }
    }

    private void HandleDayNightCircle()
    {
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
    }

    private void HandleWeatherChange()
    {
        // start new weather
        if (timer >= nextWeatherStartTime)
        {
            StartRandomWeather();
            nextWeatherStartTime += nextWeatherLastingTime + weatherChangeTimeStep + UnityEngine.Random.Range(0f, weatherChangeTimeStepVariation);
        }

        // reset weather
        if (timer >= nextWeatherStopTime)
        {
            ChangeWeather(0);
            nextWeatherLastingTime = weatherLastingTime + UnityEngine.Random.Range(0f, weatherLastingVariation);
            nextWeatherStopTime = nextWeatherStartTime + nextWeatherLastingTime;
        }
    }

    private void AISpawning()
    {
        // spawn ai randomly
        if (timer >= nextAISpawnTime)
        {
            // spawn AIs randomly
            for (int i = 0; i < aiSpawnQuantity; i++)
            {
                SpawnAIRandomly();
            }
            nextAISpawnTime += aiSpawnWaveTimeStep;
        }
    }

    private void LootboxSpawning()
    {
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

    private void MerchantSpawning()
    {
        // spawn merchant randomly
        if (!ObjectPool.objectPool.isAllMerchantActive && timer >= nextMerchantSpawnTime)
        {
            // spawn merchants randomly
            /*for (int i = 0; i < merchantSpawnQuantity; i++)
            {
                SpawnMerchantRandomly();
            }*/
            StartCoroutine(Co_SpawnMerchantRandomly());
            nextMerchantSpawnTime += merchantSpawnWaveTimeStep;
        }
    }
    #endregion

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
        nextObjectiveActivationTime = objectiveActivationTimeStep;
        nextObjectiveActivationAnnouncementTime = nextObjectiveActivationTime - objectiveAnnouncementTimeBeforeActivation;
    }
    #endregion

    #region Objectives
    private void InitializeObjectiveList()
    {
        objectiveList = new Objective[objectiveParent.childCount];
        for (int i = 0; i < objectiveParent.childCount; i++)
        {
            objectiveList[i] = objectiveParent.GetChild(i).GetComponent<Objective>();
        }
    }

    private void AnnounceIncommingObjectiveActivationEvent()
    {
        // announce the global event
        Game_Network.GlobalEventAnnouncementObjectiveActivationNotification(PV);
    }

    private void AnnounceIncommingActiveObjective(int activationIndex)
    {
        // announce each individual objectives 

    }

    private void AnnounceObjectivesHaveBeenActivated()
    {
        // announce the global event
        Game_Network.GlobalEventAnnouncementObjectiveActivation(PV);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="activateQuantity"> How many objectives will be activated. </param>
    private void ActivateObjectivesRandomly()
    {
        // if there is some objective active, skip it

        // get the indices for all non-active objectives in the list
        List<int> nonActiveIndices = new List<int>();
        for (int i = 0; i < objectiveList.Length; i++)
        {
            if (objectiveList[i].isActive)
            {
                continue;
            }
            nonActiveIndices.Add(i);
        }

        if (nonActiveIndices.Count <= 0)
        {
            Debug.Log("All objectives are active!");
            return;
        }

        // quantity
        var calculatedQuantityBasedOnTime = CalulateActivateQuantityBasedOnGameTime();
        for (int i = 0; i < ((calculatedQuantityBasedOnTime <= nonActiveIndices.Count) ? calculatedQuantityBasedOnTime : nonActiveIndices.Count); i++)
        {
            int randIndex = nonActiveIndices[UnityEngine.Random.Range(0, nonActiveIndices.Count)];
            objectiveList[randIndex].ResetAndActivateObjective();
            nonActiveIndices.Remove(randIndex);
        }
    }

    private int CalulateActivateQuantityBasedOnGameTime()
    {
        if (timer < 400f)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }
    #endregion

    #region Day-Night Cycle
    private void NightToDay()
    {
        Game_Network.NightToDay(PV);

        // update all players' vision
        foreach (Transform player in spawnedPlayerParent)
        {
            Player_NetWork.UpdateVision(player.GetComponent<PhotonView>(), 35f);
        }
    }

    private void DayToNight()
    {
        Game_Network.DayToNight(PV);

        // update all players' vision
        foreach (Transform player in spawnedPlayerParent)
        {
            Player_NetWork.UpdateVision(player.GetComponent<PhotonView>(), 15f);
        }
    }
    #endregion

    #region Weather
    public void StartRandomWeather()
    {
        var randVal = UnityEngine.Random.Range(0, 2 + 2 + 1 + 1 + 1 + 1);
        if (randVal < 2)
        {
            ChangeWeather(0);
        }
        else if (randVal < 4)
        {
            ChangeWeather(1);
        }
        else if (randVal < 5)
        {
            ChangeWeather(2);
        }
        else if (randVal < 6)
        {
            ChangeWeather(3);
        }
        else if (randVal < 7)
        {
            ChangeWeather(4);
        }
        else if (randVal < 8)
        {
            ChangeWeather(5);
        }
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

    #region Merchant
    public void SpawnMerchant(Vector2 pos, byte type, int areaIndex, out bool succeed)
    {
        int requestedMerchantIndex = ObjectPool.objectPool.RequestMerchantIndexFromPool();
        if (requestedMerchantIndex != -1)
        {
            Game_Network.SpawnMerchant(PV, requestedMerchantIndex, pos);
            MerchantWorld merchantWorld = ObjectPool.objectPool.pooledMerchant[requestedMerchantIndex].GetComponent<MerchantWorld>();
            merchantWorld.SetMerchant(areaIndex, type);
            merchantWorld.Expire();
            succeed = true;
        }
        else
        {
            succeed = false;
        }
    }

    public void SpawnMerchantRandomly()
    {
        var randAreaIndex = GetRandomSpawnArea_Merchant();
        if (randAreaIndex == -1)
            return;

        var randSpawnArea = merchantSpawnAreas[randAreaIndex].Item1;
        var spawnPoint = randSpawnArea.GetRandomPointFromArea();

        // try spawn a merchant spawner
        Transform spawner = Instantiate(ItemAssets.itemAssets.pfMerchantSpawner, spawnPoint, Quaternion.identity, spawnedMerchantParent);
        spawner.GetComponent<MerchantSpawner>().SpawnMerchant(randAreaIndex);
    }

    public void SpawnMerchantRandomlyInArea(int whichArea)
    {
        // check if the selected area is full
        if (merchantSpawnAreas[whichArea].Item2 <= merchantSpawnAreas[whichArea].Item3)
        {
            return;
        }

        var randSpawnPoint = merchantSpawnAreas[whichArea].Item1;
        var spawnPoint = randSpawnPoint.GetRandomPointFromArea();

        // try spawn a loot box spawner
        Transform spawner = Instantiate(ItemAssets.itemAssets.pfMerchantSpawner, spawnPoint, Quaternion.identity, spawnedMerchantParent);
        spawner.GetComponent<MerchantSpawner>().SpawnMerchant(whichArea);
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
        int requestedItemWorldIndex = ObjectPool.objectPool.RequestItemWorldIndexFromPool();

        if (requestedItemWorldIndex != -1)
        {
            Game_Network.SpawnItemWorld(PV, requestedItemWorldIndex, pos, itemID, amount, durability);
        }
    }

    public void DropItem(Vector2 pos, short itemID, short amount, short durability)
    {
        int requestedItemWorldIndex = ObjectPool.objectPool.RequestItemWorldIndexFromPool();

        if (requestedItemWorldIndex != -1)
        {
            Game_Network.DropItemWorld(PV, requestedItemWorldIndex, Utilities.Math.GetRandomDegree(), pos, itemID, amount, durability);
        }
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

            // check if the area is a singular spawn area
            if (area.isSingular)
            {
                lootBoxSpawnAreas.Add((area, 1, 0));
                continue;
            }

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

            // check if the area is a singular spawn area
            if (area.isSingular)
            {
                aiSpawnAreas.Add((area, 1));
                continue;
            }

            // calculate max capacity based on density
            int maxCapacity = Mathf.RoundToInt(((area.GetAreaSize()) / 11.25f) * aiSpawnDensity);

            // check override value
            if (area.overrideSpawnDensity != 0)
                maxCapacity = Mathf.RoundToInt(((area.GetAreaSize()) / 11.25f) * area.overrideSpawnDensity);

            aiSpawnAreas.Add((area, maxCapacity));
        }

        // merchant
        merchantSpawnAreas = new List<(SpawnArea, int, int)>();
        foreach (Transform child in merchantSpawnAreaParent)
        {
            var area = child.GetComponent<SpawnArea>();

            // check if the area is a singular spawn area
            if (area.isSingular)
            {
                merchantSpawnAreas.Add((area, 1, 0));
                continue;
            }

            // calculate max capacity based on density
            int maxCapacity = Mathf.RoundToInt(((area.GetAreaSize()) / 11.25f) * merchantSpawnDensity);

            // check override value
            if (area.overrideSpawnDensity != 0)
                maxCapacity = Mathf.RoundToInt(((area.GetAreaSize()) / 11.25f) * area.overrideSpawnDensity);

            merchantSpawnAreas.Add((area, maxCapacity, 0));
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
            int randIndex = indices[UnityEngine.Random.Range(0, indices.Count)];

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
            int randIndex = indices[UnityEngine.Random.Range(0, indices.Count)];

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

    private int GetRandomSpawnArea_Merchant()
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < merchantSpawnAreas.Count; i++)
        {
            indices.Add(i);
        }

        // randomly select an available area
        for (int i = 0; i < merchantSpawnAreas.Count; i++)
        {
            int randIndex = indices[UnityEngine.Random.Range(0, indices.Count)];

            // check if the area is full
            if (merchantSpawnAreas[randIndex].Item3 < merchantSpawnAreas[randIndex].Item2)
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

    private IEnumerator Co_SpawnMerchantRandomly() 
    {
        for (int i = 0; i < merchantSpawnQuantity; i++)
        {
            SpawnMerchantRandomly();
            yield return new WaitForSecondsRealtime(SPAWN_TIME_STEP);
        }
    }
    #endregion

    #region Chat
    public void DisplayVoicePlayingStatus(bool isPlaying)
    {
        
    }
    public void DisplayVoiceVolume(float volume)
    {

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
