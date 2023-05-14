using UnityEngine;
using Photon.Pun;
using NetworkCalls;

public class AIWeaponController : MonoBehaviour
{
    public DamageInfo damageInfo;

    private PhotonView _PV;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();

        damageInfo = new DamageInfo
        {
            damageAmount = 10f,
            KnockBackDist = 1f,
            damageType = DamageInfo.DamageType.Physics,
        };
    }

    public void Attack(Transform target)
    {
        AI_NetWork.Attack(_PV, target);
    }
}
