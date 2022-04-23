using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileWorld : MonoBehaviour
{
    private Projectile _projectile;
    private PhotonView _PV;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RemovePhysics();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        if (target != null && collision.gameObject.CompareTag("EnemyPlayer"))
        {
            // remove physics
            RemovePhysics();

            // deal dmg
            NetworkCalls.Character.DealProjectileDamage(_PV, target.transform.parent.GetComponent<PhotonView>().ViewID);

            // stick to the target
            if (_projectile.isSticky)
            {
                transform.parent = target.transform;
            }
        }

        // TODO: Explosion coroutine: wait for explosiveTime -> check explosiveRadius -> deal dmg
    }

    public void Perish()
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

    private void RemovePhysics()
    {
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());
    }
}
