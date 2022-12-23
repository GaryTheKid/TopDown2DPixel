using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ExplosionFX : MonoBehaviour
{
    private const float MIN_EXPLOSION_DAMAGE_RATIO = 0.2f;

    [SerializeField] private GameObject _parent;
    [SerializeField] private SpriteRenderer _grenadeSprite;

    private ProjectileWorld _projectileWorld;
    private Projectile _projectile;
    private float _explosionRadius;

    private void Awake()
    {
        _projectileWorld = _parent.GetComponent<ProjectileWorld>();
        _projectile = _projectileWorld.GetProjectile();
        _explosionRadius = _projectile.explosiveRadius;
        GetComponent<CircleCollider2D>().radius = _explosionRadius;
    }

    public void SetWorldRotation()
    {
        _grenadeSprite.sprite = null;
        transform.parent = GameManager.singleton.FXParent;
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    public void DestroySelf()
    {
        Destroy(_parent);
        Destroy(gameObject);
    }

    public void TurnOffParentBehaviors()
    {
        _parent.GetComponent<TrailRenderer>().enabled = false;
        _parent.GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.transform;
        var targetPV = target.parent.GetComponent<PhotonView>();

        if (targetPV == null)
            return;

        // calculate distance       
        var distanceFromCenter = Vector2.Distance(target.position, transform.position);

        // tiers of dmg
        var dmgRatio = 0f;
        if (distanceFromCenter <= _explosionRadius)
            dmgRatio = 1f - (distanceFromCenter / _explosionRadius);

        // set minimal damage
        if (dmgRatio < MIN_EXPLOSION_DAMAGE_RATIO)
            dmgRatio = MIN_EXPLOSION_DAMAGE_RATIO;

        // deal dmg
        if (_projectile.damageInfo.damageAmount > 0f)
            NetworkCalls.Player_NetWork.DealProjectileDamage(_projectileWorld.GetAttackPV(), targetPV.ViewID, dmgRatio, _projectile.projectileID);
    }
}