using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Utilities;

public class RPC_ItemWorld : MonoBehaviour
{
    [PunRPC]
    void RPC_SetItem(short itemID, short amount, short durability)
    {
        transform.parent = GameManager.gameManager.spawnedItemParent;
        gameObject.name = "ItemWorld " + GetComponent<PhotonView>().ViewID.ToString();

        ItemWorld itemWorld = GetComponent<ItemWorld>();

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
    }

    [PunRPC]
    void RPC_ItemWorldAddForce(float dropDirAngle)
    {
        var dropDirV2 = Math.DegreeToVector2(dropDirAngle);
        GetComponent<Rigidbody2D>().AddForce(dropDirV2 * 1.5f, ForceMode2D.Impulse);
    }

    [PunRPC]
    void RPC_ItemWorldExpire()
    {
        ItemWorld itemWorld = GetComponent<ItemWorld>();

        // play animation
        itemWorld.animator.SetTrigger("Expire");
    }

    [PunRPC]
    void RPC_DestroyItemWorld()
    {
        Destroy(gameObject);
    }
}
