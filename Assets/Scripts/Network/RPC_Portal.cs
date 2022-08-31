using UnityEngine;
using Photon.Pun;

public class RPC_Portal : MonoBehaviour
{
    private Teleport_Single teleport;

    private void Awake()
    {
        teleport = GetComponent<Teleport_Single>();
    }

    [PunRPC]
    void RPC_TeleportPrepare(int teleportTargetID)
    {
        teleport.animator.ResetTrigger("Reset");
        teleport.animator.SetTrigger("Preparing");
        teleport.teleportTarget = PhotonView.Find(teleportTargetID).transform;
    }

    [PunRPC]
    void RPC_TeleportReset()
    {
        teleport.animator.SetTrigger("Reset");
        teleport.teleportTarget = null;
    }

    [PunRPC]
    void RPC_TeleportFunctioning()
    {
        if (teleport.teleportTarget != null)
        {
            teleport.animator.SetTrigger("Functioning");
            teleport.teleportTarget.position = teleport.destination.position;
            teleport.teleportTarget = null;
            teleport.collider2D.enabled = false;
            teleport.animator.ResetTrigger("Reset");
            teleport.isRecharging = true;
            teleport.Recharge();
        }
    }
}
