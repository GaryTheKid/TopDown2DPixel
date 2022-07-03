using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxSpawner : MonoBehaviour
{
    public float spawnDelay;
    private bool hasObstacle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hasObstacle = true;
    }

    public void SpawnLootBox(int whichArea)
    {
        StartCoroutine(Co_SpawnLootBox(whichArea));
    }

    IEnumerator Co_SpawnLootBox(int whichArea)
    {
        // check if there is any obstacle
        var timer = 0f;
        while (timer <= spawnDelay)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            if (hasObstacle)
            {
                GameManager.gameManager.SpawnLootBoxRandomlyInArea(whichArea);
                Destroy(gameObject);
            }
        }

        // spawn Loot box
        bool isSpawnSucceed = false;
        GameManager.gameManager.SpawnLootBox(transform.position, whichArea, out isSpawnSucceed);

        // spawn area available count + 1
        if (isSpawnSucceed)
        {
            var area = GameManager.gameManager.lootBoxSpawnAreas[whichArea];
            var updatedArea = (area.Item1, area.Item2, area.Item3 + 1);
            GameManager.gameManager.lootBoxSpawnAreas[whichArea] = updatedArea;
        }

        // destroy
        Destroy(gameObject);
    }
}
