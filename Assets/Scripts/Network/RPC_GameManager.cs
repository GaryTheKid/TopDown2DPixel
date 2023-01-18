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


    [PunRPC]
    void RPC_UpdatePlayerInfo(int viewID, string name)
    {
        Transform player = PhotonView.Find(viewID).transform;
        player.name = name;
        player.transform.parent = GameManager.singleton.spawnedPlayerParent;
        player.GetComponent<PlayerNetworkController>().playerID = name;
        player.GetComponent<PlayerNetworkController>().scoreboardTag = GameManager.singleton.SpawnScoreboardTag(name);
    }

    [PunRPC]
    void RPC_SpawnLootBox(int index, Vector2 pos)
    {
        var obj = ObjectPool.objectPool.pooledLootBoxes[index].gameObject;
        obj.SetActive(true);
        obj.transform.position = pos;
    }

    [PunRPC]
    void RPC_SpawnAI(int index, Vector2 pos)
    {
        var obj = ObjectPool.objectPool.pooledAI[index].gameObject;
        obj.SetActive(true);
        obj.GetComponent<AIStatsController>().Respawn();
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
        while (GameManager.singleton.globalLight.intensity > 0f)
        {
            intensity -= Time.deltaTime * LIGHT_UPDATE_SPEED;
            GameManager.singleton.globalLight.intensity = intensity;
            day.color = new Color(1f, 1f, 1f, intensity);
            Night.color = new Color(1f, 1f, 1f, 1f - intensity);
            yield return new WaitForEndOfFrame();
        }
        GameManager.singleton.globalLight.intensity = 0f;
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
    }
}
