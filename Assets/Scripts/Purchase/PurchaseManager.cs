using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
{
    public static PurchaseManager singleton;

    [SerializeField] private Text _goldText;
    [SerializeField] private Text _gemText;
    [SerializeField] private List<Button> _purchaseUIElements;

    private IEnumerator Co_SyncPlayerCustomProperty_Gold;
    private IEnumerator Co_SyncPlayerCustomProperty_Gem;

    private void Start()
    {
        StartCoroutine(GetCloud_Currency());
    }

    private IEnumerator GetCloud_Currency()
    {
        yield return new WaitUntil(() =>
        {
            return CloudCommunicator.singleton.hasDataSynced;
        });
        
        PlayerSettings.singleton.Gold = CloudCommunicator.singleton.gold;
        PlayerSettings.singleton.Gem = CloudCommunicator.singleton.gem;
        
        UpdateGoldUI();
        UpdateGemUI();
    }

    // TODO: secure transactions, no leak and miss (freeze everything until receive call backs)

    // transmission flow:
    // UI trigger purchase request => wait for purchase callback|=> if (succeed): request sync to cloud => wait for cloud response|=> if (succeed): Re-enable interaction UI, show success UI, Update Gold Text UI
    //    ^                                           |      |_>|=> if (fail):    Re-enable UI, show failed UI                 |_>|=> if (fail)   : Re-enable interaction UI, show fail to sync cloud UI, (Undo purchase, save to local (how to avoid missing purchase tho?)) 
    //    |__________Disable interaction UI___________|                                     |                                                                |
    //    |                                                                                 |                                                                | 
    //    |_________________________________________________Enable interaction UI___________|                                                                |
    //    |                                                                                                                                                  |
    //    |_________________________________________________________________________________________________Enable interaction UI____________________________|


    public void GainGold(int newGold)
    {
        DisableAllPurchaseUIs();

        CloudCommunicator.singleton.gold += newGold;
        SyncPlayerCustomProperty_Gold((bool isSuccessful) => 
        {
            if (isSuccessful)
            {
                UpdateGoldUI();
            }
            else
            {
                PopFailUI();
            }
        } );
    }

    public void GainGem(int newGem)
    {
        DisableAllPurchaseUIs();

        CloudCommunicator.singleton.gem += newGem;
        SyncPlayerCustomProperty_Gem((bool isSuccessful) =>
        {
            if (isSuccessful)
            {
                UpdateGemUI();
            }
            else
            {
                PopFailUI();
            }
        });
    }

    public void SpendGold(int spentGold)
    {
        DisableAllPurchaseUIs();

        CloudCommunicator.singleton.gold -= spentGold;
        if (CloudCommunicator.singleton.gold < 0)
        {
            CloudCommunicator.singleton.gold = 0;
        }
        SyncPlayerCustomProperty_Gold((bool isSuccessful) =>
        {
            if (isSuccessful)
            {
                UpdateGoldUI();
            }
            else
            {
                PopFailUI();
            }
        });
        UpdateGoldUI();
    }

    public void SpendGem(int spentGem)
    {
        DisableAllPurchaseUIs();

        CloudCommunicator.singleton.gem -= spentGem;
        if (CloudCommunicator.singleton.gem < 0)
        {
            CloudCommunicator.singleton.gem = 0;
        }
        SyncPlayerCustomProperty_Gem((bool isSuccessful) =>
        {
            if (isSuccessful)
            {
                UpdateGemUI();
            }
            else
            {
                PopFailUI();
            }
        });
        UpdateGemUI();
    }

    public void UpdateGoldUI()
    {
        _goldText.text = CloudCommunicator.singleton.gold.ToString();
        EnableAllPurchaseUIs();
    }

    public void UpdateGemUI()
    {
        _gemText.text = CloudCommunicator.singleton.gem.ToString();
        EnableAllPurchaseUIs();
    }

    private void PopFailUI()
    {
        CloudCommunicator.singleton.PopCloudConnectionFailUI();
        EnableAllPurchaseUIs();
    }

    private void DisableAllPurchaseUIs()
    {
        foreach (Button uiElement in _purchaseUIElements)
        {
            uiElement.interactable = false;
        }
    }

    private void EnableAllPurchaseUIs()
    {
        foreach (Button uiElement in _purchaseUIElements)
        {
            uiElement.interactable = true;
        }
    }

    private void SyncPlayerCustomProperty_Gold(Action<bool> OnGoldSyncToCloudCallback)
    {
        if (Co_SyncPlayerCustomProperty_Gold != null)
        {
            StopCoroutine(Co_SyncPlayerCustomProperty_Gold);
            Co_SyncPlayerCustomProperty_Gold = null;
        }
        Co_SyncPlayerCustomProperty_Gold = SyncPlayerCustomProperty_Gold_Co(OnGoldSyncToCloudCallback);
        StartCoroutine(Co_SyncPlayerCustomProperty_Gold);
    }

    private void SyncPlayerCustomProperty_Gem(Action<bool> OnGemSyncToCloudCallback)
    {
        if (Co_SyncPlayerCustomProperty_Gem != null)
        {
            StopCoroutine(Co_SyncPlayerCustomProperty_Gem);
            Co_SyncPlayerCustomProperty_Gem = null;
        }
        Co_SyncPlayerCustomProperty_Gem = SyncPlayerCustomProperty_Gem_Co(OnGemSyncToCloudCallback);
        StartCoroutine(Co_SyncPlayerCustomProperty_Gem);
    }

    private IEnumerator SyncPlayerCustomProperty_Gold_Co(Action<bool> OnGoldSyncToCloudCallback)
    {
        yield return new WaitForSeconds(CloudCommunicator.singleton.singleRequestSyncCD);

        SetPlayerData_Gold(OnGoldSyncToCloudCallback);
    }

    private IEnumerator SyncPlayerCustomProperty_Gem_Co(Action<bool> OnGemSyncToCloudCallback)
    {
        yield return new WaitForSeconds(CloudCommunicator.singleton.singleRequestSyncCD);

        SetPlayerData_Gold(OnGemSyncToCloudCallback);
    }

    private void SetPlayerData_Gold(Action<bool> OnGoldSyncToCloudCallback)
    {
        PlayerSettings.singleton.Gold = CloudCommunicator.singleton.gold;
        CloudCommunicator.singleton.SyncPlayerCustomDataToCloud("Currency_Gold", PlayerSettings.singleton.Gold, OnGoldSyncToCloudCallback);
    }

    private void SetPlayerData_Gem(Action<bool> OnGemSyncToCloudCallback)
    {
        PlayerSettings.singleton.Gem = CloudCommunicator.singleton.gem;
        CloudCommunicator.singleton.SyncPlayerCustomDataToCloud("Currency_Gem", PlayerSettings.singleton.Gem, OnGemSyncToCloudCallback);
    }
}
