/* Last Edition: 06/10/2022
* Author: Chongyang Wang
* Collaborators: 
* 
* Description: 
*   The script attached to the loot box in the world
*/

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using NetworkCalls;

public class LootBoxWorld : MonoBehaviour
{
    /*public static LootBoxWorld SpawnLootBoxWorld(Vector3 postion, short lootBoxWorldID, int areaIndex)
    {
        Transform transform = Instantiate(ItemAssets.itemAssets.pfLootBoxWorld, postion, Quaternion.identity, GameManager.gameManager.spawnedLootBoxParent);
        transform.name = "L" + lootBoxWorldID.ToString();
        LootBoxWorld lootBoxWorld = transform.GetComponent<LootBoxWorld>();
        lootBoxWorld.lootBoxWorldID = lootBoxWorldID;
        lootBoxWorld.areaIndex = areaIndex;
        return lootBoxWorld;
    }*/

    [SerializeField] private Text interactionText;

    public PhotonView PV;
    public int areaIndex;
    public Animator animator;

    private IEnumerator Expire_Co;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
    }

    public void DisplayInteractionText()
    {
        interactionText.gameObject.SetActive(true);
    }

    public void HideInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }

    public void OpenLootBox()
    {
        // disable colliders
        foreach (Collider2D collider in GetComponents<Collider2D>()) collider.enabled = false;

        // stop expiring
        if (Expire_Co != null)
        {
            StopCoroutine(Expire_Co);
            Expire_Co = null;
        }

        // network call
        LootBox_NetWork.OpenLootBox(PV);
    }

    public void SetLootBox(int areaIndex)
    {
        LootBox_NetWork.SetLootBox(PV, areaIndex);
    }

    public void SpawnRandomItem()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            short randItemID = (short)UnityEngine.Random.Range(1, ItemAssets.itemAssets.itemDic.Count + 1);
            short amount = 1;
            if (ItemAssets.itemAssets.itemDic[randItemID].itemType == Item.ItemType.Consumable)
            {
                amount = (short)UnityEngine.Random.Range(1, 5);
            }
            GameManager.gameManager.SpawnItem(transform.position, randItemID, amount);
        }
    }

    public void Expire()
    {
        if (Expire_Co == null)
        {
            Expire_Co = Co_WaitForExpire();
            StartCoroutine(Expire_Co);
        }
    }
    IEnumerator Co_WaitForExpire()
    {
        yield return new WaitForSecondsRealtime(GameManager.gameManager.lootBoxWorldLifeTime);

        // disable colliders
        foreach (Collider2D collider in GetComponents<Collider2D>()) collider.enabled = false;

        // network call
        try
        {
            LootBox_NetWork.Expire(PV);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        // clear corouting
        Expire_Co = null;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
        //LootBox_NetWork.DestroyLootBox(PV);
    }
}
