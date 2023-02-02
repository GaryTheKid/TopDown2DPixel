/* Last Edition: 01/18/2023
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The UI element for item being vendered by the merchant.
 * Last Edition:
 *   Just Created.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_VenderItem : MonoBehaviour
{
    #region Fields
    // public fields
    public Item venderItem;
    public short amount;
    public short durability;
    public short price;

    // private fields
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image durabilityIcon;
    [SerializeField] private Text amountText;
    [SerializeField] private Text durabilityText;
    [SerializeField] private Text priceText;
    [SerializeField] private GameObject priceIcon;
    #endregion

    #region Custom Functions
    /// <summary>
    /// Set the item attribute info
    /// </summary>
    public void SetItemAttributes(short randItemID, short amount, short durability, short price)
    {
        venderItem = ItemAssets.itemAssets.itemDic[randItemID];
        this.amount = amount;
        this.durability = durability;
        this.price = price;
        priceIcon.SetActive(true);
        UpdateUI();
    }

    /// <summary>
    /// Clear the current item's UI elements
    /// </summary>
    public void ClearItemUI()
    {
        venderItem = null;
        itemIcon.sprite = ItemAssets.itemAssets.ui_icon_none;
        durabilityIcon.sprite = ItemAssets.itemAssets.ui_icon_none;
        amountText.text = "";
        durabilityText.text = "";
        priceText.text = "";
        priceIcon.SetActive(false);
    }

    /// <summary>
    /// Update ui based on the item data
    /// </summary>
    public void UpdateUI()
    {
        itemIcon.sprite = venderItem.GetSprite();
        durabilityIcon.sprite = venderItem.GetDurabilitySprite();
        if (amount > 1)
        {
            amountText.text = amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
        if (durability > 0)
        {
            durabilityText.text = durability.ToString();
        }
        else
        {
            durabilityText.text = "";
        }

        switch (venderItem.itemType)
        {
            case Item.ItemType.MeleeWeapon:
            case Item.ItemType.RangedWeapon:
            case Item.ItemType.ChargableRangedWeapon:
            case Item.ItemType.Scroll:
                amountText.text = "";
                break;

            case Item.ItemType.Consumable:
            case Item.ItemType.ThrowableWeapon:
                durabilityText.text = "";
                break;
        }

        priceText.text = price.ToString();
    }
    #endregion
}