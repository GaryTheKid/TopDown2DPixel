using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HolySacrificeFX : MonoBehaviour
{
    private DamageInfo _damageInfo;
    private PhotonView _attackerPV;

    public void SetFXInfo(PhotonView PV, DamageInfo dmgInfo)
    {
        _attackerPV = PV;
        _damageInfo = dmgInfo;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.transform;

        if (target.gameObject.name != "HitBox")
            return;

        var targetPV = target.parent.GetComponent<PhotonView>();

        if (targetPV == null)
            return;

        // deal dmg
        NetworkCalls.Player_NetWork.DealDamage(_attackerPV, targetPV.ViewID, _damageInfo);
    }
}
