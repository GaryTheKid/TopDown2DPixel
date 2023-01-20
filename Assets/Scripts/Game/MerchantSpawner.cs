/* Last Edition: 01/18/2023
 * Author: Chongyang Wang
 * Collaborators: 
 * References:
 * 
 * Description: 
 *   Network spawner for merchant
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantSpawner : MonoBehaviour
{
    public float spawnDelay;
    private bool hasObstacle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hasObstacle = true;
    }

    public void SpawnMerchant(int whichArea)
    {
        StartCoroutine(Co_SpawnMerchant(whichArea));
    }

    IEnumerator Co_SpawnMerchant(int whichArea)
    {
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
        byte merchantType = (byte)UnityEngine.Random.Range(0, 1);

        // spawn merchant
        GameManager.singleton.SpawnMerchant(transform.position, merchantType, whichArea, out bool isSpawnSucceed);

        // spawn area available count + 1
        if (isSpawnSucceed)
        {
            var area = GameManager.singleton.merchantSpawnAreas[whichArea];
            var updatedArea = (area.Item1, area.Item2, area.Item3 + 1);
            GameManager.singleton.merchantSpawnAreas[whichArea] = updatedArea;
        }

        // destroy
        Destroy(gameObject);
    }
}