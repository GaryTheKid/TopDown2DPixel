/* Last Edition: 01/18/2023
 * Author: Chongyang Wang
 * Collaborators: 
 * References:
 * 
 * Description: 
 *   RPC calls for merchant
 */
using UnityEngine;
using Photon.Pun;

public class RPC_Merchant : MonoBehaviour
{
    private MerchantWorld merchant;

    private void Awake()
    {
        merchant = GetComponent<MerchantWorld>();
    }

    [PunRPC]
    void RPC_SetVenderItems(byte itemIndex, short randItemID, short amount, short durability, short price)
    {
        switch (itemIndex)
        {
            case 0:
                merchant.venderItem_1.SetItemAttributes(randItemID, amount, durability, price);
                break;

            case 1:
                merchant.venderItem_2.SetItemAttributes(randItemID, amount, durability, price);
                break;

            case 2:
                merchant.venderItem_3.SetItemAttributes(randItemID, amount, durability, price);
                break;
        }
    }

    [PunRPC]
    void RPC_RemoveVenderItem(byte itemIndex)
    {
        switch (itemIndex)
        {
            case 0:
                merchant.venderItem_1.ClearItemUI();
                break;

            case 1:
                merchant.venderItem_2.ClearItemUI();
                break;

            case 2:
                merchant.venderItem_3.ClearItemUI();
                break;
        }
    }

    [PunRPC]
    void RPC_MerchantExpire()
    {
        MerchantWorld merchantWorld = GetComponent<MerchantWorld>();

        // release area available count
        var area = GameManager.singleton.merchantSpawnAreas[merchantWorld.areaIndex];
        var updatedArea = (area.Item1, area.Item2, area.Item3 - 1);
        GameManager.singleton.merchantSpawnAreas[merchantWorld.areaIndex] = updatedArea;

        // play animation
        merchantWorld.animator.SetTrigger("Expire");
    }

    [PunRPC]
    void RPC_DestroyMerchant()
    {
        Destroy(gameObject);
    }
}
