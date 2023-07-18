using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCalls;

public class PlayerVisionController : MonoBehaviour
{
    private const float VISION_UPDATE_SPEED = 20f;

    [SerializeField] private Transform vision;

    private IEnumerator _updateVision_Co;

    private void Start()
    {
        UpdateVision(GetComponent<PlayerStatsController>().playerStats.dayVision);
    }

    public void UpdateVision(float newVisionRadius)
    {
        if (_updateVision_Co != null)
        {
            StopCoroutine(_updateVision_Co);
            _updateVision_Co = null;
        }
        _updateVision_Co = Co_UpdateVision(newVisionRadius);
        StartCoroutine(_updateVision_Co);
    }

    private IEnumerator Co_UpdateVision(float newVisionRadius)
    {
        var rad = vision.localScale.x;

        // check if the new radius is larger or smaller
        if (rad > newVisionRadius)
        {
            while (vision.localScale.x > newVisionRadius)
            {
                rad -= Time.deltaTime * VISION_UPDATE_SPEED;
                vision.localScale = new Vector3(rad, rad);
                yield return new WaitForEndOfFrame();
            }
            vision.localScale = new Vector3(newVisionRadius, newVisionRadius);
        }
        else if (rad < newVisionRadius)
        {
            while (vision.localScale.x < newVisionRadius)
            {
                rad += Time.deltaTime * VISION_UPDATE_SPEED;
                vision.localScale = new Vector3(rad, rad);
                yield return new WaitForEndOfFrame();
            }
            vision.localScale = new Vector3(newVisionRadius, newVisionRadius);
        }
    }
}
