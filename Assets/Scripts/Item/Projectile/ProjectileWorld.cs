using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWorld : MonoBehaviour
{
    private Projectile projectile;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());
    }

    public void Perish()
    {
        StartCoroutine(Co_Perish(projectile.lifeTime));
    }

    private IEnumerator Co_Perish(float lifeTime)
    {
        yield return new WaitForSecondsRealtime(lifeTime);
        Destroy(gameObject);
    }

    public void SetProjectile(Projectile projectile)
    {
        this.projectile = projectile;
    }
}
