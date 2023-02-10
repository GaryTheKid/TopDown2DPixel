using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NetworkCalls;

public class ActivationTrigger : MonoBehaviour
{
    public bool isActive;

    [SerializeField] private DeployableObject_World _deployableObject_World;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_deployableObject_World.isLocked)
            return;

        if (collision.gameObject.name != "HitBox")
            return;

        PhotonView targetPV = collision.GetComponentInParent<PhotonView>();
        if (targetPV != null && !collision.gameObject.CompareTag("DeployIndicator") && !collision.gameObject.CompareTag("Deployable_Detection") && !collision.gameObject.CompareTag("Deployable_Activation"))
        {
            ActivateDeployable(targetPV);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_deployableObject_World.isLocked)
            return;

        if (!_deployableObject_World.IsDeployableDeactivatable())
            return;

        if (collision.gameObject.name != "HitBox")
            return;

        PhotonView targetPV = collision.GetComponentInParent<PhotonView>();
        if (targetPV != null && !collision.gameObject.CompareTag("DeployIndicator") && !collision.gameObject.CompareTag("Deployable_Detection") && !collision.gameObject.CompareTag("Deployable_Activation"))
        {
            DeactivateDeployable(targetPV);
        }
    }

    public PhotonView GetDeployablePV()
    {
        return _deployableObject_World.GetDeployerPV();
    }

    public void ActivateDeployable(PhotonView targetPV)
    {
        _deployableObject_World.ShowActivateVisual();
        if (targetPV.IsMine)
        {
            Deployable_Network.ActivateDeployable(_deployableObject_World.GetThisPV());
        }
    }

    public void DeactivateDeployable(PhotonView targetPV)
    {
        _deployableObject_World.ShowDeactivateVisual();
        if (targetPV.IsMine)
        {
            Deployable_Network.DeactivateDeployable(_deployableObject_World.GetThisPV());
        }
    }
}