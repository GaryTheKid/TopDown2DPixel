using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ActivationTrigger : MonoBehaviour
{
    public bool isActive;

    [SerializeField] private DeployableObject_World _deployableObject_World;

    public PhotonView GetDeployablePV()
    {
        return _deployableObject_World.GetDeployerPV();
    }

    public void ActivateDeployable()
    {
        _deployableObject_World.ShowActivateVisual();
    }

    public void DeactiveDeployable()
    {
        _deployableObject_World.ShowDeactivateVisual();
    }
}
