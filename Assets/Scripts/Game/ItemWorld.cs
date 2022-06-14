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
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 postion, Item item, short itemWorldID, short amount = 1)
    {
        Transform transform = Instantiate(ItemAssets.itemAssets.pfItemWorld, postion, Quaternion.identity, GameManager.gameManager.spawnedItemParent);
        transform.name = itemWorldID.ToString();
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        item.amount = amount;
        itemWorld.SetItem(item, itemWorldID);
        return itemWorld;
    }

    public static ItemWorld SpawnItemWorld(Vector3 postion, Item item, short itemWorldID, short durability, short amount = 1)
    {
        Transform transform = Instantiate(ItemAssets.itemAssets.pfItemWorld, postion, Quaternion.identity, GameManager.gameManager.spawnedItemParent);
        transform.name = itemWorldID.ToString();
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        item.amount = amount;
        item.durability = durability;
        itemWorld.SetItem(item, itemWorldID);
        return itemWorld;
    }

    public short itemWorldID;
    private Item item;
    private IEnumerator expire_Co;
    private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Text amountText;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetItem(Item item, short itemWorldID)
    {
        this.item = item;
        this.itemWorldID = itemWorldID;
        spriteRenderer.sprite = item.GetSprite();
        if (item.amount > 1)
        {
            amountText.text = item.amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
    }

    public Item GetItem()
    {
        return item;
    }

    public void Expire(float time)
    {
        expire_Co = Co_Expire(time);
        StartCoroutine(expire_Co);
    }
    IEnumerator Co_Expire(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        foreach (Collider2D collider in GetComponents<Collider2D>()) Destroy(collider);
        GameManager.gameManager.ReleaseLootBoxWorldId(itemWorldID);
        animator.SetTrigger("Expire");
        expire_Co = null;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
