using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GhostInteracter : MonoBehaviour
{
    private PhotonView _PV;

    private void Awake()
    {
        _PV = GetComponentInParent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_PV.IsMine)
            return;

        // interact with bush
        Bush bush = collision.GetComponent<Bush>();
        if (bush != null && !bush.GetComponent<Animator>().GetBool("Reveal"))
        {
            bush.isCharacterInside = true;
            bush.RevealBush();
        }

        // interact with deployable
        DetectionTrigger detectionTrigger = collision.GetComponent<DetectionTrigger>();
        if (detectionTrigger != null && !detectionTrigger.isDetected)
        {
            detectionTrigger.isDetected = true;
            detectionTrigger.ShowDetectionVisual();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_PV.IsMine)
            return;

        // interact with bush
        Bush bush = collision.GetComponent<Bush>();
        if (bush != null && bush.GetComponent<Animator>().GetBool("Reveal"))
        {
            bush.isCharacterInside = false;
            bush.HideBush();
        }

        // interact with deployable
        DetectionTrigger detectionTrigger = collision.GetComponent<DetectionTrigger>();
        if (detectionTrigger != null && detectionTrigger.isDetected)
        {
            detectionTrigger.isDetected = false;
            detectionTrigger.HideDetectionVisual();
        }
    }
}
