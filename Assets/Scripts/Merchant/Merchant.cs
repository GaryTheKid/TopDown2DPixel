using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Merchant : MonoBehaviour
{
    [SerializeField] private GameObject TradeUI;
    [SerializeField] private GameObject InsufficientResourceInfo;
    [SerializeField] private GameObject SuccessfulPurchaseInfo;

    public void RevealTradeUI()
    {
        TradeUI.SetActive(true);
    }

    public void HideTradeUI()
    {
        TradeUI.SetActive(false);
    }

    public void ShowInsufficientResourceInfo()
    {
        InsufficientResourceInfo.SetActive(true);

        // TODO: play merchant animation
    }

    public void ShowSuccessfulPurchaseInfo()
    {
        SuccessfulPurchaseInfo.SetActive(true);

        // TODO: play merchant animation
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
