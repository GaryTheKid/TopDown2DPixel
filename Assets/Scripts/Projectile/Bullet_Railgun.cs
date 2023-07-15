/* Last Edition Date: 02/10/2023
 * Author: Chongyang Wang
 * Collaborators: 
 * References:
 * 
 * Description: 
 *   Projectile - Bullet - Railgun
 * Last Edition:
 *   Just created.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Railgun : Projectile
{
    #region Constructors
    /// <summary>
    /// Constructor
    /// </summary>
    public Bullet_Railgun()
    {
        projectileID = 7;

        speed = 30f;
        maxDist = 100f;
        explosiveRadius = 0f;
        lifeTime = 2f;
        isSticky = false;
        canDirectHit = true;
        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 30f,
            knockBackDist = 2.5f,
        };
    }
    #endregion


    #region Custom Functions
    public override Transform GetProjectilePrefab()
    {
        return ItemAssets.itemAssets.projBullet_Railgun;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.ui_icon_railGunBullet;
    }
    #endregion
}
