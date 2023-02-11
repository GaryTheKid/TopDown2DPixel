/* Last Edition: 01/10/2023
 * Author: Chongyang Wang
 * Collaborators: 
 * References:
 * 
 * Description: 
 *   The merchant selling items to players
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using NetworkCalls;

public class MerchantWorld : MonoBehaviour
{
    #region Fields
    public enum MerchantType
    {
        Normal,

    }
    public MerchantType merchantType;

    public UI_VenderItem venderItem_1;
    public UI_VenderItem venderItem_2;
    public UI_VenderItem venderItem_3;
    public int areaIndex;
    public Animator animator;

    [SerializeField] private GameObject _TradeUI;
    [SerializeField] private GameObject _InsufficientResourceInfo;
    [SerializeField] private GameObject _SuccessfulPurchaseInfo;
    [SerializeField] private GameObject _shadow;

    private PhotonView _PV;
    private IEnumerator _Expire_Co;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
    }
    #endregion

    #region Custom Functions
    /// <summary>
    /// Display UI for Tradable items
    /// </summary>
    public void RevealTradeUI()
    {
        _TradeUI.SetActive(true);
    }

    /// <summary>
    /// Hide UI for Tradable items
    /// </summary>
    public void HideTradeUI()
    {
        _TradeUI.SetActive(false);
    }

    /// <summary>
    /// Display message after player does not have sufficient resource to purchase
    /// </summary>
    public void ShowInsufficientResourceInfo()
    {
        _InsufficientResourceInfo.SetActive(true);

        // TODO: play merchant animation
    }

    /// <summary>
    /// Display message after player successfully purchased
    /// </summary>
    public void ShowSuccessfulPurchaseInfo()
    {
        _SuccessfulPurchaseInfo.SetActive(true);

        // TODO: play merchant animation
    }

    /// <summary>
    /// Initialize the items for vendering
    /// </summary>
    public void SetVenderItems()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // set item attributes for all 3 items
            for (byte i = 0; i < 3; i++)
            {
                // roll a item id
                short randItemID = (short)UnityEngine.Random.Range(1, ItemAssets.itemAssets.itemDic.Count);
                Item item = ItemAssets.itemAssets.itemDic[randItemID];

                // set item amount and durability 
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

                    case Item.ItemType.DeployableWeapon:
                        amount += (short)UnityEngine.Random.Range(1, 2);
                        break;
                }

                // set item price based on the item attributes
                short price = 0;
                switch (item.itemType)
                {
                    case Item.ItemType.MeleeWeapon:
                    case Item.ItemType.RangedWeapon:
                    case Item.ItemType.ChargableRangedWeapon:
                    case Item.ItemType.Scroll:
                        price = (short)(durability * ItemAssets.itemAssets.itemCostDic[randItemID]);
                        break;

                    case Item.ItemType.Consumable:
                    case Item.ItemType.ThrowableWeapon:
                    case Item.ItemType.DeployableWeapon:
                        price = (short)(amount * ItemAssets.itemAssets.itemCostDic[randItemID]);
                        break;
                }

                // call rpc to set it
                Merchant_NetWork.SetVenderItems(_PV, i, randItemID, amount, durability, price);
            }
        }
    }

    public void SetMerchant(int areaIndex, byte type)
    {
        this.areaIndex = areaIndex;
        merchantType = (MerchantType)type;
        // TODO: RPC set merchant appears based on type;


        SetVenderItems();
    }

    public void Expire()
    {
        if (_Expire_Co == null)
        {
            _Expire_Co = Co_WaitForExpire();
            StartCoroutine(_Expire_Co);
        }
    }
    IEnumerator Co_WaitForExpire()
    {
        yield return new WaitForSecondsRealtime(GameManager.singleton.merchantLifeTime);

        // disable colliders
        foreach (Collider2D collider in GetComponents<Collider2D>()) collider.enabled = false;

        // network call
        try
        {
            Merchant_NetWork.Expire(_PV);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        // clear corouting
        _Expire_Co = null;
    }

    public void DestroySelf()
    {
        gameObject.SetActive(false);

        // reset sprite
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var col = spriteRenderer.color;
        col.a = 1f;
        spriteRenderer.color = col;

        // re-enable colliders
        foreach (Collider2D collider in GetComponents<Collider2D>()) collider.enabled = true;

        // re-enable shadow
        _shadow.SetActive(true);

        // inform object pool
        ObjectPool.objectPool.isAllMerchantActive = false;
    }
    #endregion
}
