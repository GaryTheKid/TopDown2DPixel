using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPC_GameManager : MonoBehaviour
{
    [PunRPC]
    void RPC_SpawnItem(Vector3 pos, short itemID)
    {
        ItemWorld.SpawnItemWorld(pos, ItemAssets.itemAssets.itemDic[itemID], GameManager.gameManager.RequestNewItemWorldId());
    }
}
