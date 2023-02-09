using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ActivationTrigger : MonoBehaviour
{
    public bool isActive;

    [SerializeField] private DeployableObject_World _deployableObject_World;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_deployableObject_World.isLocked)
            return;

        PhotonView targetPV = collision.GetComponentInParent<PhotonView>();
        if (targetPV != null && !collision.gameObject.CompareTag("DeployIndicator"))
        {
            ActivateDeployable();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_deployableObject_World.isLocked)
            return;

        if (!_deployableObject_World.IsDeployableDeactivatable())
            return;

        PhotonView targetPV = collision.GetComponentInParent<PhotonView>();
        if (targetPV != null && !collision.gameObject.CompareTag("DeployIndicator"))
        {
            DeactivateDeployable();
        }
    }

    public PhotonView GetDeployablePV()
    {
        return _deployableObject_World.GetDeployerPV();
    }

    public void ActivateDeployable()
    {
        _deployableObject_World.ShowActivateVisual();
    }

    public void DeactivateDeployable()
    {
        _deployableObject_World.ShowDeactivateVisual();
    }
}