/* Last Edition: 06/13/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   All RPC calls for the master client game manager.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPC_GameManager : MonoBehaviour
{
    [PunRPC]
    void RPC_SpawnLootBox(Vector3 pos)
    {
        short requestedID = GameManager.gameManager.RequestNewLootBoxWorldId();
        if (requestedID != -1)
            LootBoxWorld.SpawnLootBoxWorld(pos, requestedID);
        else
            DebugGUI.debugGUI.ShowDebugTag("Loot box number in world has reached MAX!", 5f);
    }

    [PunRPC]
    void RPC_SpawnItem(Vector3 pos, short itemID)
    {
        short requestedID = GameManager.gameManager.RequestNewItemWorldId();
        if(requestedID != -1)
            ItemWorld.SpawnItemWorld(pos, ItemAssets.itemAssets.itemDic[itemID], requestedID);
        else
            DebugGUI.debugGUI.ShowDebugTag("Item number in world has reached MAX!", 5f);
    }

    [PunRPC]
    void RPC_SpawnItems(Vector3 pos, short itemID, short amount)
    {
        short requestedID = GameManager.gameManager.RequestNewItemWorldId();
        if (requestedID != -1)
            ItemWorld.SpawnItemWorld(pos, ItemAssets.itemAssets.itemDic[itemID], requestedID, amount);
        else
            DebugGUI.debugGUI.ShowDebugTag("Item number in world has reached MAX!", 5f);
    }
}
