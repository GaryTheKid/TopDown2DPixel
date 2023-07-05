using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NetworkCalls;

public class Teleport_Single : MonoBehaviour
{
    private const float RECHARGE_CD = 5f;

    public AudioSource sfx_Portal;
    public AudioSource sfx_Exit;
    public Transform teleportTarget;
    public Animator animator;
    public Transform destination;
    public Collider2D cd2D;
    public bool isRecharging;

    private PhotonView _PV;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        cd2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isRecharging && collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            animator.ResetTrigger("Reset");
            animator.SetTrigger("Preparing");
            MapObj_Network.Prepare(_PV, collision.transform.parent.GetComponent<PhotonView>().ViewID);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isRecharging && collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            animator.SetTrigger("Reset");
            MapObj_Network.Reset(_PV);
        }
    }

    public void Functioning()
    {
        MapObj_Network.Functioning(_PV);
    }

    public void Recharge()
    {
        StartCoroutine(Co_Recharge());
    }

    private IEnumerator Co_Recharge()
    {
        yield return new WaitForSecondsRealtime(RECHARGE_CD);
        animator.SetTrigger("RechargingComplete");
        cd2D.enabled = true;
        isRecharging = false;
    }
}
