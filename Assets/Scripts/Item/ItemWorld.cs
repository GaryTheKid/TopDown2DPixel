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
    public static ItemWorld SpawnItemWorld(Vector3 postion, Item item, short itemWorldID)
    {
        Transform transform = Instantiate(ItemAssets.itemAssets.pfItemWorld, postion, Quaternion.identity, GameManager.gameManager.spawnedItemParent);
        transform.name = itemWorldID.ToString();
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item, itemWorldID);
        return itemWorld;
    }

    public static ItemWorld SpawnItemWorld(Vector3 postion, Item item, short itemWorldID, short amount)
    {
        Transform transform = Instantiate(ItemAssets.itemAssets.pfItemWorld, postion, Quaternion.identity, GameManager.gameManager.spawnedItemParent);
        transform.name = itemWorldID.ToString();
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        item.amount = amount;
        itemWorld.SetItem(item, itemWorldID);
        return itemWorld;
    }

    public short itemWorldID;
    private Item item;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Text amountText;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
