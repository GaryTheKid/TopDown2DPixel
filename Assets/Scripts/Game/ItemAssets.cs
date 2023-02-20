using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets itemAssets;

    private void Awake()
    {
        itemAssets = this;
        InitItemDics();
        InitSkillIconDic();
    }

    // prefab for weapon Equipped
    [Header("Equipment Prefabs")]
    public Transform pfHands;
    public Transform pfSword;
    public Transform pfAxe;
    public Transform pfBow;
    public Transform pfGun_AK;
    public Transform pfGun_M4;
    public Transform pfGun_Pistol;
    public Transform pfGun_Rifle;
    public Transform pfGun_M3;
    public Transform pfGun_SMAX;
    public Transform pfGun_Prototype03;
    public Transform pfSmokeGrenade;
    public Transform pfHEGrenade;
    public Transform pfImpactGrenade;
    public Transform pfScroll;
    public Transform pfDeployableMine;

    // prefab for projectile
    [Header("Projectile Prefabs")]
    public Transform projArrow;
    public Transform projBullet_SemiAuto;
    public Transform projBullet_Rifle;
    public Transform projBullet_Shotgun;
    public Transform projBullet_Railgun;
    public Transform projSmokeGrenade;
    public Transform projHEGrenade;
    public Transform projImpactGrenade;

    // prefab for deployable object
    [Header("Projectile Prefabs")]
    public Transform deployableObj_Mine;

    // prefab for spell
    [Header("Spell Prefabs")]
    public Transform pfSpell_Tornado;
    public Transform pfSpell_Meteor;

    // prefab for item world
    [Header("ItemWorld Prefab")]
    public Transform pfItemWorld;

    // prefab for loot box world
    [Header("LootBoxWorld Prefab")]
    public Transform pfLootBoxWorld;

    // prefab for merchant world
    [Header("Merchant Spawner Prefab")]
    public Transform pfMerchantWorld;

    // prefab for loot box spawner
    [Header("LootBox Spawner Prefab")]
    public Transform pfLootBoxSpawner;

    // prefab for merchant spawner
    [Header("Merchant Spawner Prefab")]
    public Transform pfMerchantSpawner;

    // prefab for ai spawner
    [Header("AI Spawner Prefab")]
    public Transform pfAISpawner;

    // weather and day/night cycle sprite
    [Header("Weather Sprites")]
    public Sprite rainningWeatherSprite;
    public Sprite WindWeatherSprite;

    // Sprite for skills 
    [Header("Skill Sprites")]
    public Dictionary<PlayerSkillController.Skills, Sprite> skillIconDic;
    public Sprite skill_Agility;
    public Sprite skill_Sturdybody;
    public Sprite skill_Regenaration;

    // merchant ui sprite
    [Header("Merchant Sprites")]
    public Sprite Merchant_Icon_Random;

    // Sprite for inventory and itemworld
    [Header("Item Sprites")]
    public Sprite handsSprite;
    public Sprite swordSprite;
    public Sprite axeSprite;
    public Sprite bowSprite;
    public Sprite gunSprite_AK;
    public Sprite gunSprite_M4;
    public Sprite gunSprite_Pistol;
    public Sprite gunSprite_Rifle;
    public Sprite gunSprite_M3;
    public Sprite gunSprite_SMAX;
    public Sprite gunSprite_Prototype03;
    public Sprite smokeGrenadeSprite;
    public Sprite HEGrenadeSprite;
    public Sprite impactGrenadeSprite;
    public Sprite healthPotionSprite;
    public Sprite bigHealthPotionSprite;
    public Sprite superHealthPotionSprite;
    public Sprite speedPotionSprite;
    public Sprite invinciblePotionSprite;
    public Sprite blinkScrollSprite;
    public Sprite stealthScrollSprite;
    public Sprite earthquakeScrollSprite;
    public Sprite meteorStrikeScrollSprite;
    public Sprite meteorRainScrollSprite;
    public Sprite tornadoScrollSprite;
    public Sprite stormScrollSprite;
    public Sprite deployableMineSprite;

    [Header("Durability Icon Sprites")]
    public Sprite ui_icon_none;
    public Sprite ui_icon_melee;
    public Sprite ui_icon_pistolBullets;
    public Sprite ui_icon_semiAutoBullets;
    public Sprite ui_icon_rifleBullet;
    public Sprite ui_icon_ShotgunBullets;
    public Sprite ui_icon_railGunBullet;
    public Sprite ui_icon_arrow;

    [Header("Item Dictionary")]
    public Dictionary<short, Item> itemDic;
    public Dictionary<short, short> itemCostDic;
    public Dictionary<short, Projectile> projectileDic;
    public Dictionary<short, DeployableObject> deployableObjDic;
    private void InitItemDics()
    {
        itemDic = new Dictionary<short, Item>
        {
            { 0, new Hands() },
            { 1, new Sword() },
            { 2, new Bow() },
            { 3, new Axe() },
            { 4, new HealthPotion() },
            { 5, new Gun_AK() },
            { 6, new Gun_M4() },
            { 7, new SpeedPotion() },
            { 8, new BigHealthPotion() },
            { 9, new InvinciblePotion() },
            { 10, new Gun_Pistol() },
            { 11, new Gun_Rifle() },
            { 12, new SuperHealthPotion() },
            { 13, new SmokeGrenade() },
            { 14, new HEGrenade() },
            { 15, new ImpactGrenade() },
            { 16, new Scroll_Blink() },
            { 17, new Scroll_Tornado() },
            { 18, new Scroll_MeteorStrike() },
            { 19, new Gun_M3() },
            { 20, new Gun_SMAX() },
            { 21, new Gun_Prototype03() },
            { 22, new Mines() }
        };

        itemCostDic = new Dictionary<short, short>
        {
            { 0, 0 },
            { 1, 5 },
            { 2, 5 },
            { 3, 4 },
            { 4, 15 },
            { 5, 2 },
            { 6, 2 },
            { 7, 60 },
            { 8, 30 },
            { 9, 80 },
            { 10, 1 },
            { 11, 5 },
            { 12, 60 },
            { 13, 20 },
            { 14, 10 },
            { 15, 10 },
            { 16, 20 },
            { 17, 20 },
            { 18, 15 },
            { 19, 5 },
            { 20, 4 },
            { 21, 10 },
            { 22, 30 }
        };

        projectileDic = new Dictionary<short, Projectile>
        {
            { 0, new Arrow() },
            { 1, new Bullet_SemiAuto() },
            { 2, new Bullet_Rifle() },
            { 3, new SmokeGrenade_Proj() },
            { 4, new HEGrenade_proj() },
            { 5, new ImpactGrenade_Proj() },
            { 6, new Bullet_Shotgun() },
            { 7, new Bullet_Railgun() }
        };

        deployableObjDic = new Dictionary<short, DeployableObject> 
        {
            { 0, new Mines_dpl() },
        };
    }

    private void InitSkillIconDic()
    {
        skillIconDic = new Dictionary<PlayerSkillController.Skills, Sprite>
        {
            { PlayerSkillController.Skills.Agility, skill_Agility },
            { PlayerSkillController.Skills.Sturdybody, skill_Sturdybody },
            { PlayerSkillController.Skills.Regenaration, skill_Regenaration },
        };
    }
}
