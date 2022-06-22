using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileWorld : MonoBehaviour
{
    [SerializeField] private FXPlayer_Projectile _FX_projectile;

    private Projectile _projectile;
    private PhotonView _PV;
    private float _dmgRatio = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // only collide non-self colliders
        if (collision.transform != _PV.transform)
        {
            if (_projectile.isSticky)
            {
                _FX_projectile.PlayStickFX();
            }
            else
            {
                _FX_projectile.PlayCollisionFX();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        if (target != null && target.transform != _PV.transform.Find("HitBox"))
        {
            // deal dmg
            if(_projectile.damageInfo.damageAmount > 0f)
                NetworkCalls.Player_NetWork.DealProjectileDamage(_PV, target.transform.parent.GetComponent<PhotonView>().ViewID, _dmgRatio);

            // check if stick to the target
            if (_projectile.isSticky)
            {
                _FX_projectile.PlayStickFX();
            }
            else
            {
                _FX_projectile.PlayHitFX();
            }
        }

        // TODO: Explosion coroutine: wait for explosiveTime -> check explosiveRadius -> deal dmg
    }

    public void PerishInTime()
    {
        StartCoroutine(Co_Perish(_projectile.lifeTime));
    }

    private IEnumerator Co_Perish(float lifeTime)
    {
        yield return new WaitForSecondsRealtime(lifeTime);
        Destroy(gameObject);
    }

    public void SetProjectile(Projectile projectile)
    {
        _projectile = projectile;
    }

    public void SetAttackerPV(PhotonView PV)
    {
        _PV = PV;
    }

    public void SetDamageRatio(float dmgRatio)
    {
        _dmgRatio = dmgRatio;
    }

    public void RemovePhysics()
    {
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());
    }
}
