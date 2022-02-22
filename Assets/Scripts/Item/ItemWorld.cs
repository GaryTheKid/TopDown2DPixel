using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 postion, Item item)
    {
        Transform transform = Instantiate(ItemAssets.itemAssets.pfItemWorld, postion, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    public static ItemWorld DropItem(Vector3 dropPos, Item item)
    {
        Vector3 randomDir = Math.GetRandomDirectionV3();
        ItemWorld itemWorld = SpawnItemWorld(dropPos + randomDir * 1.2f, item);
        itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 1.5f, ForceMode2D.Impulse);
        return itemWorld;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Text amountText;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
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
