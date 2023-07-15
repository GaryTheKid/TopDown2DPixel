using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MineExplosionFX : MonoBehaviour
{
    private const float MIN_EXPLOSION_DAMAGE_RATIO = 0.2f;

    [SerializeField] private DeployableObject_World _deployableWorld;
    [SerializeField] private SpriteRenderer[] _parentVisuals;

    private DeployableObject _deployableObj;
    private float _explosionRadius;

    private void Awake()
    {
        _deployableObj = _deployableWorld.GetDeployableObject();
        _explosionRadius = _deployableObj.explosiveRadius;
        GetComponent<CircleCollider2D>().radius = _explosionRadius;
    }

    public void DestroySelf()
    {
        gameObject.SetActive(false);
        _deployableWorld.DestroySelf();
    }

    public void TurnOffParent()
    {
        _deployableWorld.isLocked = true;
        _deployableWorld.HideDetectionVisual();
        _deployableWorld.ShowDeactivateVisual();
        _deployableWorld.TurnOffActivationLight();
        foreach (var visual in _parentVisuals)
        {
            visual.color = new Color(visual.color.r, visual.color.g, visual.color.b, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.transform;
        
        if (target.gameObject.name != "HitBox")
            return;

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

        DamageInfo newDamageInfo = _deployableObj.damageInfo;
        newDamageInfo.damageAmount = _deployableObj.damageInfo.damageAmount * dmgRatio;

        // deal dmg
        if (newDamageInfo.damageAmount > 0f)
            NetworkCalls.Player_NetWork.DealDamage(_deployableWorld.GetDeployerPV(), targetPV.ViewID, newDamageInfo);
    }
}
