/* Last Edition: 06/03/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The Spawner for picking spawn position for lootbox.
 * Last Edition:
 *   Just Created.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public float spawnDelay;
    private bool hasObstacle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hasObstacle = true;
    }

    public void SpawnAI(int whichArea)
    {
        StartCoroutine(Co_SpawnAI(whichArea));
    }

    IEnumerator Co_SpawnAI(int whichArea)
    {
        // check if there is any obstacle
        var timer = 0f;
        while (timer <= spawnDelay)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            if (hasObstacle)
            {
                GameManager.singleton.SpawnAIRandomlyInArea(whichArea);
                Destroy(gameObject);
            }
        }

        // spawn AI
        GameManager.singleton.SpawnAI(transform.position);

        // destroy
        Destroy(gameObject);
    }
}
