using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShakeTrigger : MonoBehaviour
{
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float shakeTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerFXController = collision.transform.parent.GetComponent<PlayerEffectController>();

        if (playerFXController != null)
        {
            playerFXController.CameraShake(shakeIntensity, shakeTime);
        }
    }
}
