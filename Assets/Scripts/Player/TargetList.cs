using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TargetList : MonoBehaviour
{
    public HashSet<int> targets;
    private PhotonView _PV;

    private void Awake()
    {
        targets = new HashSet<int>();
        _PV = GetComponent<PhotonView>();
    }

    public void AddTarget(int targetID)
    {
        NetworkCalls.Character.LockTarget(_PV, targetID);
    }

    public void RemoveTarget(int targetID)
    {
        NetworkCalls.Character.UnlockTarget(_PV, targetID);
    }
}
