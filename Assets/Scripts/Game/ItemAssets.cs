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
    public Transform pfSmokeGrenade;
    public Transform pfHEGrenade;
    public Transform pfImpactGrenade;
    public Transform pfScroll;

    // prefab for weapon Equipped
    [Header("Projectile Prefabs")]
    public Transform projArrow;
    public Transform projBullet_SemiAuto;
    public Transform projBullet_Rifle;
    public Transform projSmokeGrenade;
    public Transform projHEGrenade;
    public Transform projImpactGrenade;

    // prefab for spell
    [Header("Spell Prefabs")]
    public Transform pfSpell_Tornado;

    // prefab for item world
    [Header("ItemWorld Prefab")]
    public Transform pfItemWorld;

    // prefab for loot box world
    [Header("LootBoxWorld Prefab")]
    public Transform pfLootBoxWorld;

    // prefab for loot box spawner
    [Header("LootBox Spawner Prefab")]
    public Transform pfLootBoxSpawner;

    // prefab for ai spawner
    [Header("AI Spawner Prefab")]
    public Transform pfAISpawner;

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
    public Sprite meteorRainScrollSprite;
    public Sprite tornadoScrollSprite;
    public Sprite stormScrollSprite;

    [Header("Durability Icon Sprites")]
    public Sprite ui_icon_none;
    public Sprite ui_icon_melee;
    public Sprite ui_icon_pistolBullets;
    public Sprite ui_icon_semiAutoBullets;
    public Sprite ui_icon_rifleBullet;
    public Sprite ui_icon_arrow;

    [Header("Item Dictionary")]
    public Dictionary<short, Item> itemDic;
    public Dictionary<short, Projectile> projectileDic;

    private void InitItemDics()
    {
        itemDic = new Dictionary<short, Item>{
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
            { 17, new Scroll_Tornado() }
        };

        projectileDic = new Dictionary<short, Projectile>{
            { 0, new Arrow() },
            { 1, new Bullet_SemiAuto() },
            { 2, new Bullet_Rifle() },
            { 3, new SmokeGrenade_Proj() },
            { 4, new HEGrenade_proj() },
            { 5, new ImpactGrenade_Proj() }
        };
    }
}
