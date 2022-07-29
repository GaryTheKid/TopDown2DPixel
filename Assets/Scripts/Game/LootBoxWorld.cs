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
    [SerializeField] private Text interactionText;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private GameObject shadow;

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
            short randItemID = (short)UnityEngine.Random.Range(1, ItemAssets.itemAssets.itemDic.Count);
            Item item = ItemAssets.itemAssets.itemDic[randItemID];
            short amount = 1;
            short durability = item.durability;
            switch (item.itemType)
            {
                case Item.ItemType.MeleeWeapon:
                    durability += (short)UnityEngine.Random.Range(-5, 3);
                    break;

                case Item.ItemType.RangedWeapon:
                case Item.ItemType.ChargableRangedWeapon:
                    durability += (short)UnityEngine.Random.Range(-10, 5);
                    break;

                case Item.ItemType.Consumable:
                    // invincible potion, speed potion, super health potion
                    if (!(item.itemID == 7 || item.itemID == 8 || item.itemID == 9))
                    {
                        amount = (short)UnityEngine.Random.Range(1, 4);
                    }
                    break;

                case Item.ItemType.ThrowableWeapon:
                    amount = (short)UnityEngine.Random.Range(1, 3);
                    break;

                case Item.ItemType.Scroll:
                    durability += (short)UnityEngine.Random.Range(1, 2);
                    break;
            }
            GameManager.gameManager.SpawnItem(transform.position, randItemID, amount, durability);
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
        gameObject.SetActive(false);

        // reset sprite
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var col = spriteRenderer.color;
        col.a = 1f;
        spriteRenderer.color = col;
        spriteRenderer.sprite = defaultSprite;

        // re-enable colliders
        foreach (Collider2D collider in GetComponents<Collider2D>()) collider.enabled = true;

        // re-enable shadow
        shadow.SetActive(true);

        // inform object pool
        ObjectPool.objectPool.isAllLootBoxActive = false;
    }
}
