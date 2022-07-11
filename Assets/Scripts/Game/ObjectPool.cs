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
    public readonly List<GameObject> pooledAI = new List<GameObject>();
    public bool isAllLootBoxActive;
    public bool isAllAIActive;

    void Awake()
    {
        objectPool = this;

        // add disabled loot box
        foreach (Transform lootBox in GameManager.gameManager.spawnedLootBoxParent)
        {
            pooledLootBoxes.Add(lootBox.gameObject);
        }

        // add disabled AI
        foreach (Transform AI in GameManager.gameManager.spawnedAIParent)
        {
            pooledAI.Add(AI.gameObject);
        }
    }

    public int RequestAIIndexFromPool()
    {
        for (int i = 0; i < pooledAI.Count; i++)
        {
            if (!pooledAI[i].activeInHierarchy)
            {
                isAllAIActive = false;
                return i;
            }
        }

        isAllAIActive = true;
        return -1;
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
