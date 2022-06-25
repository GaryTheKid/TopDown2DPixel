using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Utilities;

public class WorldInteracter : MonoBehaviour
{
    public List<LootBoxWorld> lootBoxesInRange;
    public List<ItemWorld> itemWorldsInRange;

    private PhotonView _PV;

    private void Awake()
    {
        _PV = GetComponentInParent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_PV.IsMine)
            return;

        // interact with loot box
        LootBoxWorld lootBoxWorld = collision.GetComponent<LootBoxWorld>();

        if (lootBoxWorld != null)
        {
            // display interaction text
            lootBoxWorld.DisplayInteractionText();

            // add to the loot box list
            lootBoxesInRange.Add(lootBoxWorld);

            // hide all the other loot box interaction text
            for (int i = 0; i < lootBoxesInRange.Count - 1; i++)
            {
                lootBoxesInRange[i].HideInteractionText();
            }
        }

        // interact with item world
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            // display interaction text
            itemWorld.DisplayInteractionText();

            // add to the item world list
            itemWorldsInRange.Add(itemWorld);

            // hide all the other loot box interaction text
            for (int i = 0; i < itemWorldsInRange.Count - 1; i++)
            {
                itemWorldsInRange[i].HideInteractionText();
            }
        }

        // trigger screen smoke
        if (collision.gameObject.layer == LayerMask.NameToLayer("ScreenSmoke"))
        {
            GetComponentInParent<PlayerEffectController>().ScreenSmokeOn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_PV.IsMine)
            return;

        // interact with loot box
        LootBoxWorld lootBoxWorld = collision.GetComponent<LootBoxWorld>();
        if (lootBoxWorld != null)
        {
            // hide interaction text
            lootBoxWorld.HideInteractionText();

            // remove from the loot box list
            lootBoxesInRange.Remove(lootBoxWorld);

            // display the next box in range
            if (lootBoxesInRange.Count > 0)
            {
                lootBoxesInRange[lootBoxesInRange.Count - 1].DisplayInteractionText();
            }
        }

        // interact with loot box
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            // hide interaction text
            itemWorld.HideInteractionText();

            // remove from the loot box list
            itemWorldsInRange.Remove(itemWorld);

            // display the next box in range
            if (itemWorldsInRange.Count > 0)
            {
                itemWorldsInRange[itemWorldsInRange.Count - 1].DisplayInteractionText();
            }
        }

        // screen smoke off
        if (collision.gameObject.layer == LayerMask.NameToLayer("ScreenSmoke"))
        {
            GetComponentInParent<PlayerEffectController>().ScreenSmokeOff();
        }
    }
}
