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
    private Animator animator;
    private IEnumerator expire_Co;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenLootBox()
    {
        StartCoroutine(Co_OpenLootBox());
    }
    IEnumerator Co_OpenLootBox()
    {
        if (expire_Co != null)
            StopCoroutine(expire_Co);
        animator.SetTrigger("Open");
        yield return new WaitForSecondsRealtime(0.8f);
        SpawnRandomItem();
        GameManager.gameManager.ReleaseLootBoxWorldId(lootBoxWorldID);
        Destroy(gameObject);
    }

    public void SpawnRandomItem()
    {
        short randItemID = (short)Random.Range(1, ItemAssets.itemAssets.itemDic.Count);
        short amount = 1;
        if (ItemAssets.itemAssets.itemDic[randItemID].itemType == Item.ItemType.Consumable)
        {
            amount = (short)Random.Range(1, 5);
        }
        GameManager.gameManager.SpawnItem(transform.position, randItemID, amount);
    }

    public void Expire(float time, int whichArea)
    {
        expire_Co = Co_Expire(time, whichArea);
        StartCoroutine(expire_Co);
    }
    IEnumerator Co_Expire(float time, int whichArea)
    {
        yield return new WaitForSecondsRealtime(time);
        foreach (Collider2D collider in GetComponents<Collider2D>()) Destroy(collider);
        GameManager.gameManager.ReleaseLootBoxWorldId(lootBoxWorldID);
        animator.SetTrigger("Expire");

        // release area available count
        var area = GameManager.gameManager.lootBoxSpawnAreas[whichArea];
        var updatedArea = (area.Item1, area.Item2, area.Item3 - 1);
        GameManager.gameManager.lootBoxSpawnAreas[whichArea] = updatedArea;

        // clear
        expire_Co = null;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
