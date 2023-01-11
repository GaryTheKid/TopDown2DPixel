/* Last Edition: 01/10/2023
 * Author: Chongyang Wang
 * Collaborators: 
 * References: CodyMonkey
 * 
 * Description: 
 *   The merchant selling items to players
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Pun;

public class Merchant : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject TradeUI;
    [SerializeField] private GameObject InsufficientResourceInfo;
    [SerializeField] private GameObject SuccessfulPurchaseInfo;

    [SerializeField] private Text itemAmountText_1;
    [SerializeField] private Text itemAmountText_2;
    [SerializeField] private Text itemAmountText_3;

    private float itemCost_1;
    private float itemCost_2;
    private float itemCost_3;
    #endregion

    /// <summary>
    /// Display UI for Tradable items
    /// </summary>
    public void RevealTradeUI()
    {
        TradeUI.SetActive(true);
    }


    /// <summary>
    /// Hide UI for Tradable items
    /// </summary>
    public void HideTradeUI()
    {
        TradeUI.SetActive(false);
    }

    /// <summary>
    /// Display message after player does not have sufficient resource to purchase
    /// </summary>
    public void ShowInsufficientResourceInfo()
    {
        InsufficientResourceInfo.SetActive(true);

        // TODO: play merchant animation
    }

    /// <summary>
    /// Display message after player successfully purchased
    /// </summary>
    public void ShowSuccessfulPurchaseInfo()
    {
        SuccessfulPurchaseInfo.SetActive(true);

        // TODO: play merchant animation
    }

    public void SpawnVenderItems()
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
            GameManager.singleton.SpawnItem(transform.position, randItemID, amount, durability);
        }
    }

    public void PurchaseItem1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // check if enough resources to purchase
            /*if ()
            {
                
            }*/
            ShowSuccessfulPurchaseInfo();
        }
    }

    public void PurchaseItem2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            ShowSuccessfulPurchaseInfo();
        }
    }

    public void PurchaseItem3(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShowSuccessfulPurchaseInfo();
        }
    }
}
