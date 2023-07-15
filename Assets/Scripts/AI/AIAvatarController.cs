using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AIAvatarController : MonoBehaviour
{
    private const float DEFAULT_HPBAR_SIZE_X = 0.4f;
    private const float DEFAULT_HPBAR_SIZE_Y = 0.6f;
    private const float DEFAULT_HPBAR_SIZE_MODIFIER = 40f;

    public GameObject[] avatars; // Array of sprite renderers representing different avatars
    public Transform hpBarParent;

    public void SetAI(byte enemyID)
    {
        // set ai name
        List<string> avatarNames = new List<string>(ItemAssets.itemAssets.enemyAvatarDic.Keys);
        gameObject.name = avatarNames[enemyID] + GetComponent<PhotonView>().ViewID.ToString();

        // set avatar
        for (byte i = 0; i < avatars.Length; i++)
        {
            bool isActive = (i == enemyID);
            var avatar = avatars[i];
            avatar.SetActive(isActive);
            if (isActive)
            {
                // set ai fields
                GetComponent<RPC_AI>().animator = avatar.GetComponent<Animator>();
                GetComponent<AIMovementController>().animator = avatar.GetComponent<Animator>();
                GetComponent<AIEffectController>().SetAIEffectFields(avatar);

                // set ai stats 
                var aiStatsClass = avatar.GetComponent<AI_Class>();
                var aiStats = GetComponent<AIStatsController>().aiStats;
                aiStats.isDead = false;
                aiStats.isInvincible = false;
                aiStats.isMovementLocked = false;
                aiStats.isWeaponLocked = false;
                aiStats.maxHp = aiStatsClass.maxHp;
                aiStats.expWorth = aiStatsClass.expWorth;
                aiStats.goldWorth = aiStatsClass.goldWorth;
                aiStats.baseSpeed = aiStatsClass.speed;
                aiStats.speedModifier = 1f;

                // set ai attack dmg info
                var aiWeaponcontroller = GetComponent<AIWeaponController>();
                aiWeaponcontroller.damageInfo.damageAmount = aiStatsClass.damageInfo.damageAmount;
                aiWeaponcontroller.damageInfo.knockBackDist = aiStatsClass.damageInfo.knockBackDist;
                aiWeaponcontroller.damageInfo.damageType = aiStatsClass.damageInfo.damageType;

                // set hp bar size
                var newHpBarSizeX = DEFAULT_HPBAR_SIZE_X * ((aiStatsClass.maxHp / DEFAULT_HPBAR_SIZE_MODIFIER) + 0.5f);
                hpBarParent.localScale = new Vector3(newHpBarSizeX, hpBarParent.localScale.y);

                // set collider size
                transform.Find("CharacterCollider").localScale = new Vector3(aiStatsClass.hitbox_scale_x, aiStatsClass.hitbox_scale_y);
                transform.Find("HitBox").localScale = new Vector3(aiStatsClass.hitbox_scale_x, aiStatsClass.hitbox_scale_y);

                // set ring/shadow size
                avatar.transform.Find("Shadow").localScale = new Vector3(aiStatsClass.ringNShadowSize_x, aiStatsClass.ringNShadowSize_y);
                avatar.transform.Find("Ring").localScale = new Vector3(aiStatsClass.ringNShadowSize_x, aiStatsClass.ringNShadowSize_y);
            }
        }
    }
}
