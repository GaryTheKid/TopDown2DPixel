using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileWorld : MonoBehaviour
{
    [SerializeField] private FXPlayer_Projectile _FX_projectile;

    private Projectile _projectile;
    private PhotonView _attackerPV;
    private float _dmgRatio = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // only collide non-self colliders
        if (collision.transform != _attackerPV.transform)
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
        // proceed effects
        GameObject target = collision.gameObject;

        // ignore layers
        if (target == null || 
            target.layer == LayerMask.NameToLayer("TornadoAttraction") ||
            target.CompareTag("Portal") ||
            target.CompareTag("Well") ||
            target.CompareTag("Bush"))
            return; 

        if (target.transform != _attackerPV.transform.Find("HitBox") && _projectile.canDirectHit)
        {
            // deal dmg
            if(_projectile.damageInfo.damageAmount > 0f)
                NetworkCalls.Player_NetWork.DealProjectileDamage(_attackerPV, target.transform.parent.GetComponent<PhotonView>().ViewID, _dmgRatio, _projectile.projectileID);

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

    public Projectile GetProjectile()
    {
        return _projectile;
    }

    public void SetProjectile(Projectile projectile)
    {
        _projectile = projectile;
    }

    public PhotonView GetAttackPV()
    {
        return _attackerPV;
    }

    public void SetAttackerPV(PhotonView PV)
    {
        _attackerPV = PV;
    }

    public void SetDamageRatio(float dmgRatio)
    {
        _dmgRatio = dmgRatio;
    }

    public void DisablePhysics()
    {
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());
    }
}
