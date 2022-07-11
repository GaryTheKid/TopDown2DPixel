using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NetworkCalls;

public class AIWorld : MonoBehaviour
{
    public PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public void SetAI()
    {
        AI_NetWork.SetAI(PV);
    }

    public void DestroySelf()
    {
        gameObject.SetActive(false);

        // inform object pool
        ObjectPool.objectPool.isAllAIActive = false;
    }
}
