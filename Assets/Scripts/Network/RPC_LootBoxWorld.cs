using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Utilities;

public class RPC_LootBoxWorld : MonoBehaviour
{
    [PunRPC]
    void RPC_SetLootBox(int areaIndex)
    {
        gameObject.name = "LootBox " + GetComponent<PhotonView>().ViewID.ToString();

        GetComponent<LootBoxWorld>().areaIndex = areaIndex;
    }

    [PunRPC]
    void RPC_OpenLootBox()
    {
        LootBoxWorld lootBoxWorld = GetComponent<LootBoxWorld>();

        // release area available count
        var area = GameManager.gameManager.lootBoxSpawnAreas[lootBoxWorld.areaIndex];
        var updatedArea = (area.Item1, area.Item2, area.Item3 - 1);
        GameManager.gameManager.lootBoxSpawnAreas[lootBoxWorld.areaIndex] = updatedArea;

        // play animation
        lootBoxWorld.animator.SetTrigger("Open");
    }

    [PunRPC]
    void RPC_LootBoxExpire()
    {
        LootBoxWorld lootBoxWorld = GetComponent<LootBoxWorld>();

        // release area available count
        var area = GameManager.gameManager.lootBoxSpawnAreas[lootBoxWorld.areaIndex];
        var updatedArea = (area.Item1, area.Item2, area.Item3 - 1);
        GameManager.gameManager.lootBoxSpawnAreas[lootBoxWorld.areaIndex] = updatedArea;

        // play animation
        lootBoxWorld.animator.SetTrigger("Expire");
    }

    [PunRPC]
    void RPC_DestroySelf()
    {
        Destroy(gameObject);
    }
}
