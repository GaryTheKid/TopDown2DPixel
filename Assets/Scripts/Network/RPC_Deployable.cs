using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPC_Deployable : MonoBehaviour
{
    private DeployableObject_World _deployableObject_World;

    private void Awake()
    {
        _deployableObject_World = GetComponent<DeployableObject_World>();
    }

    [PunRPC]
    void RPC_ActivateDeployable()
    {
        _deployableObject_World.Activate();
    }

    [PunRPC]
    void RPC_DeactivateDeployable()
    {
        _deployableObject_World.Deactivate();
    }
}
