/* Last Edition: 06/10/2022
* Author: Chongyang Wang
* Collaborators: 
* 
* Description: 
*   The script attached to the loot box in the world
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using NetworkCalls;

public class LootBoxWorld : MonoBehaviour
{
    public static LootBoxWorld SpawnLootBoxWorld(Vector3 postion, short lootBoxWorldID)
    {
        Transform transform = Instantiate(ItemAssets.itemAssets.pfLootBoxWorld, postion, Quaternion.identity, GameManager.gameManager.spawnedLootBoxParent);
        transform.name = "L" + lootBoxWorldID.ToString();
        LootBoxWorld lootBoxWorld = transform.GetComponent<LootBoxWorld>();
        lootBoxWorld.lootBoxWorldID = lootBoxWorldID;
        return lootBoxWorld;
    }

    public short lootBoxWorldID;

    public void SpawnRandomItem()
    {
        short randItemID = (short)Random.Range(1, ItemAssets.itemAssets.itemDic.Count);
        short amount = 1;
        if (ItemAssets.itemAssets.itemDic[randItemID].itemType == Item.ItemType.Consumable)
        {
            amount = (short)Random.Range(1, 5);
        }
        GameManager.gameManager.SpawnItem(transform.position, randItemID);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
