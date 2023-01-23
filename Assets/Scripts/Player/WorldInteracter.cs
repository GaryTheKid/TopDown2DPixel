/* Last Edition: 07/03/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The script attached to the character collider for the world objs interactions.
 * Last Edition:
 *   Just Created.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Utilities;

public class WorldInteracter : MonoBehaviour
{
#if PLATFORM_ANDROID
    [SerializeField] private UI_MobileInput _ui_MobileInput;
#endif

    public List<LootBoxWorld> lootBoxesInRange;
    public List<ItemWorld> itemWorldsInRange;
    public Well wellInRange;
    public MerchantWorld merchantInRange;
    public UI_VenderItem venderItem_1;
    public UI_VenderItem venderItem_2;
    public UI_VenderItem venderItem_3;

    private PhotonView _PV;
    private PCInputActions _inputActions;
    private PlayerInventoryController _playerInventoryController;
    private PlayerResourceController _playerResourceController;

    private bool _isInteractionButtonActivated;
    private IEnumerator _Co_ExitingSmoke;

    private void Awake()
    {
        _PV = GetComponentInParent<PhotonView>();
        _inputActions = GetComponentInParent<PlayerInputActions>().inputActions;
        _playerInventoryController = GetComponentInParent<PlayerInventoryController>();
        _playerResourceController = GetComponentInParent<PlayerResourceController>();
    }

    private void OnDisable()
    {
        // close all interactions when dead
        ClearAll();
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

        // interact with well
        wellInRange = collision.GetComponent<Well>();
        if (wellInRange != null && wellInRange.isUsable)
        {
            wellInRange.DisplayInteractionText();
        }

        // interact with bush
        Bush bush = collision.GetComponent<Bush>();
        if (bush != null)
        {
            bush.RevealBush();
        }

        // interact with merchant
        merchantInRange = collision.GetComponent<MerchantWorld>();
        if (merchantInRange != null)
        {
            // disable the equipment shortcut for merchant interactions
            _playerInventoryController.UnloadEquipmentQuickCast();

            // load trade interaction input actions
            _inputActions.Player.EquipmentQuickCast_1.performed += PurchaseItem1;
            _inputActions.Player.EquipmentQuickCast_2.performed += PurchaseItem2;
            _inputActions.Player.EquipmentQuickCast_3.performed += PurchaseItem3;

            // load merchant's vendered items
            venderItem_1 = merchantInRange.venderItem_1;
            venderItem_2 = merchantInRange.venderItem_2;
            venderItem_3 = merchantInRange.venderItem_3;

            // show trade interaction UI
            merchantInRange.RevealTradeUI();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
#if PLATFORM_ANDROID
        // swap right pad
        if (!_isInteractionButtonActivated && (collision.GetComponent<LootBoxWorld>() != null || collision.GetComponent<ItemWorld>() != null))
        {
            _ui_MobileInput.ActivateInteractionButton();
            _isInteractionButtonActivated = true;
        }
#endif

        // trigger screen smoke
        if (collision.gameObject.layer == LayerMask.NameToLayer("ScreenSmoke"))
        {
            if (_Co_ExitingSmoke != null)
            {
                StopCoroutine(_Co_ExitingSmoke);
                _Co_ExitingSmoke = null;
            }

            GetComponentInParent<PlayerEffectController>().ScreenSmokeOn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy)
            return;

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

#if PLATFORM_ANDROID
            // swap right pad
            _ui_MobileInput.DeactivateInteractionButton();
            _isInteractionButtonActivated = false;
#endif
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

#if PLATFORM_ANDROID
            // swap right pad
            _ui_MobileInput.DeactivateInteractionButton();
            _isInteractionButtonActivated = false;
#endif
        }

        // interact with well
        wellInRange = collision.GetComponent<Well>();
        if (wellInRange != null)
        {
            wellInRange.HideInteractionText();
            wellInRange = null;
        }

        // interact with bush
        Bush bush = collision.GetComponent<Bush>();
        if (bush != null)
        {
            bush.HideBush();
        }

        // screen smoke off
        if (collision.gameObject.layer == LayerMask.NameToLayer("ScreenSmoke"))
        {
            if (_Co_ExitingSmoke == null)
            {
                _Co_ExitingSmoke = Co_ExitingSmoke();
                StartCoroutine(_Co_ExitingSmoke);
            }
        }

        // interact with merchant
        merchantInRange = collision.GetComponent<MerchantWorld>();
        if (merchantInRange != null)
        {
            // unload trade interaction input actions
            _inputActions.Player.EquipmentQuickCast_1.performed -= PurchaseItem1;
            _inputActions.Player.EquipmentQuickCast_2.performed -= PurchaseItem2;
            _inputActions.Player.EquipmentQuickCast_3.performed -= PurchaseItem3;

            // unload merchant's vendered items
            venderItem_1 = null;
            venderItem_2 = null;
            venderItem_3 = null;

            // re-enable the equipment shortcut for merchant interactions
            _playerInventoryController.LoadEquipmentQuickCast();

            // hide trade interaction UI
            merchantInRange.HideTradeUI();
        }
    }

    public void PurchaseItem1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // check if has item available
            if (venderItem_1 == null)
                return;

            // check if has enough resources
            if (_playerResourceController.GetCurrentGold() >= venderItem_1.price)
            {
                // get item
                Item item = venderItem_1.venderItem;
                Item itemCopy = (Item)Common.GetObjectCopyFromInstance(item);
                itemCopy.amount = venderItem_1.amount;
                itemCopy.durability = venderItem_1.durability;
                itemCopy.price = venderItem_1.price;

                // add item to the inventory
                _playerInventoryController.AddItem(itemCopy);

                // pay the resources
                _playerResourceController.LoseGold(venderItem_1.price);

                // show purchase info
                merchantInRange.ShowSuccessfulPurchaseInfo();
            }
            else
            {
                // show insufficient resource info
                merchantInRange.ShowInsufficientResourceInfo();
            }
        }
    }

    public void PurchaseItem2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // check if has item available
            if (venderItem_2 == null)
                return;

            // check if has enough resources
            if (_playerResourceController.GetCurrentGold() >= venderItem_2.price)
            {
                // get item
                Item item = venderItem_2.venderItem;
                Item itemCopy = (Item)Common.GetObjectCopyFromInstance(item);
                itemCopy.amount = venderItem_2.amount;
                itemCopy.durability = venderItem_2.durability;
                itemCopy.price = venderItem_2.price;

                // add item to the inventory
                _playerInventoryController.AddItem(itemCopy);

                // pay the resources
                _playerResourceController.LoseGold(venderItem_2.price);

                // show purchase info
                merchantInRange.ShowSuccessfulPurchaseInfo();
            }
            else
            {
                // show insufficient resource info
                merchantInRange.ShowInsufficientResourceInfo();
            }
        }
    }

    public void PurchaseItem3(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // check if has item available
            if (venderItem_3 == null)
                return;

            // check if has enough resources
            if (_playerResourceController.GetCurrentGold() >= venderItem_3.price)
            {
                // get item
                Item item = venderItem_3.venderItem;
                Item itemCopy = (Item)Common.GetObjectCopyFromInstance(item);
                itemCopy.amount = venderItem_3.amount;
                itemCopy.durability = venderItem_3.durability;
                itemCopy.price = venderItem_3.price;

                // add item to the inventory
                _playerInventoryController.AddItem(itemCopy);

                // pay the resources
                _playerResourceController.LoseGold(venderItem_3.price);

                // show purchase info
                merchantInRange.ShowSuccessfulPurchaseInfo();
            }
            else
            {
                // show insufficient resource info
                merchantInRange.ShowInsufficientResourceInfo();
            }
        }
    }

    private IEnumerator Co_ExitingSmoke()
    {
        // wait
        yield return new WaitForSecondsRealtime(0.3f);

        // turn off smoke
        GetComponentInParent<PlayerEffectController>().ScreenSmokeOff();

        // clear
        _Co_ExitingSmoke = null;
    }

    public void ClearAll()
    {
        GetComponentInParent<PlayerEffectController>()?.ScreenSmokeOff();
        lootBoxesInRange.Clear();
        itemWorldsInRange.Clear();
        wellInRange = null;
    }
}
