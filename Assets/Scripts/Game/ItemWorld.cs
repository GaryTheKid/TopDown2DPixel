/* Last Edition: 04/28/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * References: CodyMonkey
 * 
 * Description: 
 *   The class attached to Item Prefab in the world
 */
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using NetworkCalls;

public class ItemWorld : MonoBehaviour
{
    /*public static ItemWorld SpawnItemWorld(Vector3 postion, Item item, short itemWorldID, short amount = 1)
    {
        Transform transform = Instantiate(ItemAssets.itemAssets.pfItemWorld, postion, Quaternion.identity, GameManager.gameManager.spawnedItemParent);
        transform.name = itemWorldID.ToString();
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        item.amount = amount;
        itemWorld.SetItem(item, itemWorldID);
        return itemWorld;
    }*/

    public SpriteRenderer spriteRenderer;
    public Text amountText;
    public Text interactionText;

    public PhotonView PV;
    public Animator animator;
    public Item item;

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

/*    public void SetItem(short itemID, short amount, short durability)
    {
        ItemWorld_Network.SetItem(PV, itemID, amount, durability);
    }*/

    public void PickItem()
    {
        // disable colliders
        foreach (Collider2D collider in GetComponents<Collider2D>()) collider.enabled = false;

        // stop expiring
        if (Expire_Co != null)
        {
            StopCoroutine(Expire_Co);
            Expire_Co = null;
        }

        // destroy it
        ItemWorld_Network.DestroyItemWorld(PV);
    }

    /*public void AddForce()
    {
        ItemWorld_Network.AddForce(PV, Utilities.Math.GetRandomDegree());
    }*/

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
        yield return new WaitForSecondsRealtime(GameManager.singleton.itemWorldLifeTime);

        // disable colliders
        foreach (Collider2D collider in GetComponents<Collider2D>()) collider.enabled = false;

        // network call
        try
        {
            ItemWorld_Network.Expire(PV);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        // clear coroutine
        Expire_Co = null;
    }

    public void DestroySelf()
    {
        gameObject.SetActive(false);

        // reset sprite
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        var col = spriteRenderer.color;
        col.a = 1f;
        spriteRenderer.color = col;

        // re-enable colliders
        foreach (Collider2D collider in GetComponents<Collider2D>()) collider.enabled = true;
    }
}
