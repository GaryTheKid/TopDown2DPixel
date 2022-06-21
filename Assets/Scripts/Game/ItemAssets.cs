using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets itemAssets;

    private void Awake()
    {
        itemAssets = this;
        InitItemDic();
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

    // prefab for weapon Equipped
    [Header("Projectile Prefabs")]
    public Transform pfArrow;
    public Transform pfBullet;

    // prefab for item world
    [Header("ItemWorld Prefab")]
    public Transform pfItemWorld;

    // prefab for loot box world
    [Header("LootBoxWorld Prefab")]
    public Transform pfLootBoxWorld;

    // prefab for loot box spawner
    [Header("LootBox Spawner Prefab")]
    public Transform pfLootBoxSpawner;

    // Sprite for inventory and itemworld
    [Header("Sprites")]
    public Sprite handsSprite;
    public Sprite swordSprite;
    public Sprite axeSprite;
    public Sprite bowSprite;
    public Sprite gunSprite_AK;
    public Sprite gunSprite_M4;
    public Sprite gunSprite_Pistol;
    public Sprite gunSprite_Rifle;
    public Sprite SmokeGrenadeSprite;

    public Sprite arrowSprite;
    public Sprite bulletSprite;

    public Sprite healthPotionSprite;
    public Sprite bigHealthPotionSprite;
    public Sprite superHealthPotionSprite;
    public Sprite speedPotionSprite;
    public Sprite invinciblePotionSprite;

    [Header("Item Dictionary")]
    public Dictionary<short, Item> itemDic;

    private void InitItemDic()
    {
        itemDic = new Dictionary<short, Item>{
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
        };
    }
}
