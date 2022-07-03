/* Last Edition: 06/14/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * References: Unity
 * 
 * Description: 
 *   The object pool for improving performances when spawn objects to the game scene.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool objectPool;
    public readonly List<GameObject> pooledLootBoxes = new List<GameObject>();
    public bool isAllLootBoxActive;

    void Awake()
    {
        objectPool = this;

        foreach (Transform lootBox in GameManager.gameManager.spawnedLootBoxParent)
        {
            pooledLootBoxes.Add(lootBox.gameObject);
        }
    }

    public int RequestLootBoxIndexFromPool()
    {
        for (int i = 0; i < pooledLootBoxes.Count; i++)
        {
            if (!pooledLootBoxes[i].activeInHierarchy)
            {
                isAllLootBoxActive = false;
                return i;
            }
        }

        isAllLootBoxActive = true;
        return -1;
    }
}
