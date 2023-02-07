using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DetectionTrigger : MonoBehaviour
{
    public bool isDetected;

    [SerializeField] private DeployableObject_World _deployableObject_World;

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
