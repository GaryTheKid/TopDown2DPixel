using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpellStrikeExplosion : MonoBehaviour
{
    private const float MIN_EXPLOSION_DAMAGE_RATIO = 0.5f;

    [SerializeField] private GameObject _parent;

    private SpellMeteor _spellMeteor;
    private float _explosionRadius;
    private List<int> targetCache;

    private void Awake()
    {
        _spellMeteor = _parent.GetComponent<SpellMeteor>();
        _explosionRadius = _spellMeteor.explosiveRadius;
        GetComponent<CircleCollider2D>().radius = _explosionRadius;
        targetCache = new List<int>();
    }

    public void SetWorldRotation()
    {
        transform.parent = GameManager.singleton.FXParent;
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    public void DestroySelf()
    {
        Destroy(_parent);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.transform;
        var targetPV = target.parent.GetComponent<PhotonView>();
        
        if (targetPV == null)
            return;

        if (targetCache.Contains(targetPV.ViewID))
            return;

        targetCache.Add(targetPV.ViewID);

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
        var damageInfo = ((Weapon)ItemAssets.itemAssets.itemDic[_spellMeteor.spellID]).damageInfo;
        if (damageInfo.damageAmount > 0f)
            NetworkCalls.Player_NetWork.DealSpellDamage(_spellMeteor.attackerPV, targetPV.ViewID, dmgRatio, _spellMeteor.spellID);
    }
}
