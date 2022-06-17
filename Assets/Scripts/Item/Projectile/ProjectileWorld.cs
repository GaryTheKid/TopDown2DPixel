using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileWorld : MonoBehaviour
{
    [SerializeField] private GameObject _hitFX;

    private Projectile _projectile;
    private PhotonView _PV;
    private float _dmgRatio = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // only collide non-self colliders
        if (collision.transform != _PV.transform)
        {
            RemovePhysics();
            if (!_projectile.isSticky)
            {
                StopAllCoroutines();
                _hitFX.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        if (target != null && target.transform != _PV.transform.Find("HitBox"))
        {
            // remove physics
            RemovePhysics();

            // deal dmg
            NetworkCalls.Character.DealProjectileDamage(_PV, target.transform.parent.GetComponent<PhotonView>().ViewID, _dmgRatio);

            // check if stick to the target
            if (_projectile.isSticky)
            {
                transform.parent = target.transform;
            }
            else
            {
                StopAllCoroutines();
                _hitFX.SetActive(true);
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

    private void RemovePhysics()
    {
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());
    }
}
