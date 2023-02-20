/* Last Edition Date: 01/23/2023
 * Author: Chongyang Wang			
 * Collaborators: 				
 * Reference: 	
 * 
 * Description: 
 *   The Spawner class for spawning merchant in the map.
 * Last Edition: 
 *   Just Created.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantSpawner : MonoBehaviour
{
    #region Fields
    // public field 
    public float spawnDelay;

    // private field
    private bool hasObstacle;
    #endregion


    #region Unity Functions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hasObstacle = true;
    }
    #endregion


    #region Custom Functions
    public void SpawnMerchant(int whichArea)
    {
        StartCoroutine(Co_SpawnMerchant(whichArea));
    }

    IEnumerator Co_SpawnMerchant(int whichArea)
    {
        // occupy the avilable spawn area
        AddToSpawnAreaInfo(whichArea);

        // check if there is any obstacle
        var timer = 0f;
        while (timer <= spawnDelay)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            if (hasObstacle)
            {
                GameManager.singleton.SpawnMerchantRandomlyInArea(whichArea);
                Destroy(gameObject);
            }
        }

        // get a random merchant type
        byte merchantType = (byte)UnityEngine.Random.Range(0, 2);

        // spawn merchant
        GameManager.singleton.SpawnMerchant(transform.position, merchantType, whichArea, out bool isSpawnSucceed);

        // spawn area available count - 1 if not valid
        if (!isSpawnSucceed)
        {
            RemoveFromSpawnAreaInfo(whichArea);
        }

        // destroy
        Destroy(gameObject);
    }

    private void AddToSpawnAreaInfo(int whichArea)
    {
        var area = GameManager.singleton.merchantSpawnAreas[whichArea];
        var updatedArea = (area.Item1, area.Item2, area.Item3 + 1);
        GameManager.singleton.merchantSpawnAreas[whichArea] = updatedArea;
    }

    private void RemoveFromSpawnAreaInfo(int whichArea)
    {
        var area = GameManager.singleton.merchantSpawnAreas[whichArea];
        var updatedArea = (area.Item1, area.Item2, area.Item3 - 1);
        GameManager.singleton.merchantSpawnAreas[whichArea] = updatedArea;
    }
    #endregion
}
