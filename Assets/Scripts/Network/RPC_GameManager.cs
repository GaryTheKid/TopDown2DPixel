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
    void RPC_SpawnLootBox(Vector2 pos, int whichArea)
    {
        short requestedID = GameManager.gameManager.RequestNewLootBoxWorldId();
        if (requestedID != -1)
        {
            var lootBoxWorld = LootBoxWorld.SpawnLootBoxWorld(pos, requestedID);
            lootBoxWorld.Expire(GameManager.gameManager.lootBoxWorldLifeTime, whichArea);
        }
        else
            DebugGUI.debugGUI.ShowDebugTag("Loot box number in world has reached MAX!", 5f);
    }

    [PunRPC]
    void RPC_SpawnItem(Vector2 pos, short itemID)
    {
        short requestedID = GameManager.gameManager.RequestNewItemWorldId();
        if (requestedID != -1)
        {
            var itemWorld = ItemWorld.SpawnItemWorld(pos, ItemAssets.itemAssets.itemDic[itemID], requestedID);
            itemWorld.Expire(GameManager.gameManager.itemWorldLifeTime);
        }
        else
            DebugGUI.debugGUI.ShowDebugTag("Item number in world has reached MAX!", 5f);
    }

    [PunRPC]
    void RPC_SpawnItems(Vector2 pos, short itemID, short amount)
    {
        short requestedID = GameManager.gameManager.RequestNewItemWorldId();
        if (requestedID != -1)
        {
            var itemWorld = ItemWorld.SpawnItemWorld(pos, ItemAssets.itemAssets.itemDic[itemID], requestedID, amount);
            itemWorld.Expire(GameManager.gameManager.itemWorldLifeTime);
        }
        else
            DebugGUI.debugGUI.ShowDebugTag("Item number in world has reached MAX!", 5f);
    }

    [PunRPC]
    void RPC_DestroyLootBox(short lootBoxID)
    {
        Destroy(GameObject.Find("L" + lootBoxID));
        GameManager.gameManager.ReleaseLootBoxWorldId(lootBoxID);
    }
}
