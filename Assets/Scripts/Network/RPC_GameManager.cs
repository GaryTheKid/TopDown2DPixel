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
using Utilities;

public class RPC_GameManager : MonoBehaviour
{
    [PunRPC]
    void RPC_UpdatePlayerInfo(int viewID, string name)
    {
        Transform player = PhotonView.Find(viewID).transform;
        player.name = name;
        player.transform.parent = GameManager.gameManager.spawnedPlayerParent;
        player.GetComponent<PlayerNetworkController>().playerID = name;
        player.GetComponent<PlayerNetworkController>().scoreboardTag = GameManager.gameManager.SpawnScoreboardTag(name);
    }

    [PunRPC]
    void RPC_SpawnLootBox(int index, Vector2 pos)
    {
        var obj = ObjectPool.objectPool.pooledLootBoxes[index].gameObject;
        obj.SetActive(true);
        obj.transform.position = pos;
    }
}
