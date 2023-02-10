using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DetectionTrigger : MonoBehaviour
{
    public bool isDetected;

    [SerializeField] private DeployableObject_World _deployableObject_World;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_deployableObject_World.isLocked)
            return;

        if (collision.gameObject.name != "HitBox")
            return;

        if (GetDeployablePV().IsMine)
            return;

        PhotonView targetPV = collision.GetComponentInParent<PhotonView>();
        if (targetPV != null && !collision.gameObject.CompareTag("DeployIndicator") && GetDeployablePV() != targetPV && !collision.gameObject.CompareTag("Deployable_Detection") && !collision.gameObject.CompareTag("Deployable_Activation"))
        {
            ShowDetectionVisual();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_deployableObject_World.isLocked)
            return;

        if (collision.gameObject.name != "HitBox")
            return;

        if (GetDeployablePV().IsMine)
            return;

        PhotonView targetPV = collision.GetComponentInParent<PhotonView>();
        if (targetPV != null && !collision.gameObject.CompareTag("DeployIndicator") && GetDeployablePV() != targetPV && !collision.gameObject.CompareTag("Deployable_Detection") && !collision.gameObject.CompareTag("Deployable_Activation"))
        {
            HideDetectionVisual();
        }
    }

    public PhotonView GetDeployablePV()
    {
        return _deployableObject_World.GetDeployerPV();
    }

    public void ShowDetectionVisual()
    {
        _deployableObject_World.ShowDetectionVisual();
    }

    public void HideDetectionVisual()
    {
        _deployableObject_World.HideDetectionVisual();
    }
}
