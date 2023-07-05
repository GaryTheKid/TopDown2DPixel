/* Last Edition: 06/13/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   All RPC calls for the master client game manager.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Utilities;
using Pathfinding;

public class RPC_GameManager : MonoBehaviour
{
    private const float LIGHT_UPDATE_SPEED = 0.8f;

    [SerializeField] private Image day;
    [SerializeField] private Image Night;
    [SerializeField] private Image Weather_Sunny;
    [SerializeField] private Image Weather_Rainning;
    [SerializeField] private Image Weather_Windy_East;
    [SerializeField] private Image Weather_Windy_West;
    [SerializeField] private Image Weather_Windy_North;
    [SerializeField] private Image Weather_Windy_South;

    [PunRPC]
    void RPC_SwitchGameState(byte newState)
    {
        GameManager.singleton.gameState = (GameManager.GameState)newState;
    }

    [PunRPC]
    void RPC_GoToResultScene()
    {
        GameManager.singleton.GoToResultScene();
    }

    [PunRPC]
    void RPC_UpdatePlayerInfo(int viewID, string name)
    {
        Transform player = PhotonView.Find(viewID).transform;
        byte playerActorNumber = (byte)PhotonView.Find(viewID).OwnerActorNr;
        player.name = name;
        player.transform.parent = GameManager.singleton.spawnedPlayerParent;
        player.GetComponent<PlayerNetworkController>().playerID = name;
        player.GetComponent<PlayerNetworkController>().scoreboardTag = GameManager.singleton.SpawnScoreboardTag(playerActorNumber);
        GameManager.singleton.scoreResults.AddNewPlayer(PhotonView.Find(viewID).OwnerActorNr);
    }

    [PunRPC]
    void RPC_UpdatePlayerPingTier(byte playerActorNumber, byte pingTier)
    {
        // add score
        foreach (Transform scoreboardTag in GameManager.singleton.scoreboardParent)
        {
            var sTag = scoreboardTag.GetComponent<ScoreboardTag>();
            if (sTag.GetActorNumber() == playerActorNumber)
            {
                sTag.SetPingTier(pingTier);
            }
        }
    }

    [PunRPC]
    void RPC_SpawnLootBox(int index, Vector2 pos)
    {
        var obj = ObjectPool.objectPool.pooledLootBoxes[index].gameObject;
        obj.SetActive(true);
        obj.transform.position = pos;
    }

    [PunRPC]
    void RPC_SpawnAI(int index, Vector2 pos, byte enemyID)
    {
        var obj = ObjectPool.objectPool.pooledAI[index].gameObject;
        obj.SetActive(true);
        obj.GetComponent<AIStatsController>().Respawn(enemyID);
        obj.transform.position = pos;
    }

    [PunRPC]
    void RPC_SpawnMerchant(int index, Vector2 pos)
    {
        var obj = ObjectPool.objectPool.pooledMerchant[index].gameObject;
        obj.SetActive(true);
        obj.transform.position = pos;
    }

    [PunRPC]
    void RPC_SpawnItemWorld(int index, Vector2 pos, short itemID, short amount, short durability)
    {
        var obj = ObjectPool.objectPool.pooledItemWorld[index].gameObject;
        obj.SetActive(true);
        ItemWorld itemWorld = obj.GetComponent<ItemWorld>();
        /*itemWorld.SetItem(itemID, amount, durability);*/

        // set item world
        obj.name = "ItemWorld " + obj.GetComponent<PhotonView>().ViewID.ToString();
        Item item = ItemAssets.itemAssets.itemDic[itemID];
        Item itemCopy = (Item)Common.GetObjectCopyFromInstance(item);
        itemWorld.item = itemCopy;
        itemWorld.item.amount = amount;
        itemWorld.item.durability = durability;
        itemWorld.interactionText.text = itemWorld.item.itemName;
        itemWorld.spriteRenderer.sprite = itemWorld.item.GetSprite();
        if (itemWorld.item.amount > 1)
        {
            itemWorld.amountText.text = itemWorld.item.amount.ToString();
        }
        else
        {
            itemWorld.amountText.text = "";
        }

        // set expire
        itemWorld.Expire();
        obj.transform.position = pos;
    }

    [PunRPC]
    void RPC_DropItemWorld(int index, float forceDir, Vector2 pos, short itemID, short amount, short durability)
    {
        var obj = ObjectPool.objectPool.pooledItemWorld[index].gameObject;
        obj.SetActive(true);
        ItemWorld itemWorld = obj.GetComponent<ItemWorld>();

        // set item world
        obj.name = "ItemWorld " + obj.GetComponent<PhotonView>().ViewID.ToString();
        Item item = ItemAssets.itemAssets.itemDic[itemID];
        Item itemCopy = (Item)Common.GetObjectCopyFromInstance(item);
        itemWorld.item = itemCopy;
        itemWorld.item.amount = amount;
        itemWorld.item.durability = durability;
        itemWorld.interactionText.text = itemWorld.item.itemName;
        itemWorld.spriteRenderer.sprite = itemWorld.item.GetSprite();
        if (itemWorld.item.amount > 1)
        {
            itemWorld.amountText.text = itemWorld.item.amount.ToString();
        }
        else
        {
            itemWorld.amountText.text = "";
        }

        // add force
        var dropDirV2 = Math.DegreeToVector2(forceDir);
        obj.GetComponent<Rigidbody2D>().AddForce(dropDirV2 * 1.5f, ForceMode2D.Impulse);

        // set expire
        itemWorld.Expire();

        obj.transform.position = pos;
    }

    [PunRPC]
    void RPC_NightToDay()
    {
        StartCoroutine(Co_IncreaseGlobalLight());
    }

    [PunRPC]
    void RPC_DayToNight()
    {
        StartCoroutine(Co_DecreaseGlobalLight());
    }

    IEnumerator Co_IncreaseGlobalLight()
    {
        var intensity = 0f;
        while (GameManager.singleton.globalLight.intensity < 1f)
        {
            intensity += Time.deltaTime * LIGHT_UPDATE_SPEED;
            GameManager.singleton.globalLight.intensity = intensity;
            day.color = new Color(1f, 1f, 1f, intensity);
            Night.color = new Color(1f, 1f, 1f, 1f - intensity);
            yield return new WaitForEndOfFrame();
        }
        GameManager.singleton.globalLight.intensity = 1f;
    }

    IEnumerator Co_DecreaseGlobalLight()
    {
        var intensity = 1f;
        while (GameManager.singleton.globalLight.intensity > 0.05f)
        {
            intensity -= Time.deltaTime * LIGHT_UPDATE_SPEED;
            GameManager.singleton.globalLight.intensity = intensity;
            day.color = new Color(1f, 1f, 1f, intensity);
            Night.color = new Color(1f, 1f, 1f, 1f - intensity);
            yield return new WaitForEndOfFrame();
        }
        GameManager.singleton.globalLight.intensity = 0.05f;
    }

    [PunRPC]
    void RPC_ChangeWeather(byte weatherCode)
    {
        // init weather buff for each player
        byte prevWeatherCode = (byte)GameManager.singleton.weather;
        foreach (Transform player in GameManager.singleton.spawnedPlayerParent)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                player.GetComponent<PlayerBuffController>().WeatherBuff(prevWeatherCode, weatherCode);
            }
        }

        // update weather code
        GameManager.singleton.weather = (GameManager.Weather)weatherCode;

        // update weather ui icon
        StartCoroutine(Co_TransitWeatherIcon(prevWeatherCode, weatherCode));
    }

    IEnumerator Co_TransitWeatherIcon(byte prevWeatherCode, byte newWeatherCode)
    {
        Image newWeatherIcon = null;
        Image prevWeatherIcon = null;

        // get icons
        switch ((GameManager.Weather)newWeatherCode)
        {
            case GameManager.Weather.Sunny:
                newWeatherIcon = Weather_Sunny;
                break;
            case GameManager.Weather.Rainning:
                newWeatherIcon = Weather_Rainning;
                break;
            case GameManager.Weather.Windy_East:
                newWeatherIcon = Weather_Windy_East;
                break;
            case GameManager.Weather.Windy_West:
                newWeatherIcon = Weather_Windy_West;
                break;
            case GameManager.Weather.Windy_North:
                newWeatherIcon = Weather_Windy_North;
                break;
            case GameManager.Weather.Windy_South:
                newWeatherIcon = Weather_Windy_South;
                break;
        }

        switch ((GameManager.Weather)prevWeatherCode)
        {
            case GameManager.Weather.Sunny:
                prevWeatherIcon = Weather_Sunny;
                break;
            case GameManager.Weather.Rainning:
                prevWeatherIcon = Weather_Rainning;
                break;
            case GameManager.Weather.Windy_East:
                prevWeatherIcon = Weather_Windy_East;
                break;
            case GameManager.Weather.Windy_West:
                prevWeatherIcon = Weather_Windy_West;
                break;
            case GameManager.Weather.Windy_North:
                prevWeatherIcon = Weather_Windy_North;
                break;
            case GameManager.Weather.Windy_South:
                prevWeatherIcon = Weather_Windy_South;
                break;
        }

        var alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * LIGHT_UPDATE_SPEED;
            newWeatherIcon.color = new Color(1f, 1f, 1f, alpha);
            prevWeatherIcon.color = new Color(1f, 1f, 1f, 1f - alpha);
            yield return new WaitForEndOfFrame();
        }

        newWeatherIcon.color = new Color(1f, 1f, 1f, 1f);
        prevWeatherIcon.color = new Color(1f, 1f, 1f, 0f);
    }

    [PunRPC]
    void RPC_LockAllPlayersActions()
    {
        foreach (Transform player in GameManager.singleton.spawnedPlayerParent)
        {
            player.GetComponent<PlayerStatsController>().LockAllActions();
        }
    }

    [PunRPC]
    void RPC_GlobalAnnouncement_ObjectiveActivation()
    {
        GlobalAnnouncementManager.singleton.PlayAnnouncement("Objectives Unlocked!");
    }

    [PunRPC]
    void RPC_GlobalAnnouncement_ObjectiveBeforeActivationNotification()
    {
        GlobalAnnouncementManager.singleton.PlayAnnouncement("Objectives active in " + GameManager.singleton.objectiveAnnouncementTimeBeforeActivation + " s");
    }

    [PunRPC]
    void RPC_GlobalAnnouncement_IndividualObjectiveBeforeActivationNotification(byte index)
    {
        GameManager.singleton.objectiveList[index].ShowIndicationBeforeActivation();
    }

    [PunRPC]
    void RPC_GlobalAnnouncement_AnnounceGameEnd()
    {
        GlobalAnnouncementManager.singleton.PlayCenterAnnouncement("Game Finish!");
    }
}
