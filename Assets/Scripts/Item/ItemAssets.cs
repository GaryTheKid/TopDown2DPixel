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
    public Transform pfSword;
    public Transform pfAxe;
    public Transform pfBow;
    public Transform pfGun_AK;
    public Transform pfGun_M4;

    // prefab for weapon Equipped
    [Header("Projectile Prefabs")]
    public Transform pfArrow;
    public Transform pfBullet;

    // prefab for item world
    [Header("ItemWorld Prefab")]
    public Transform pfItemWorld;

    // Sprite for inventory and itemworld
    [Header("Sprites")]
    public Sprite swordSprite;
    public Sprite axeSprite;
    public Sprite bowSprite;
    public Sprite gunSprite_AK;
    public Sprite gunSprite_M4;

    public Sprite arrowSprite;
    public Sprite bulletSprite;

    public Sprite healthPotionSprite;

    [Header("Item Dictionary")]
    public Dictionary<short, Item> itemDic;

    private void InitItemDic()
    {
        itemDic = new Dictionary<short, Item>{
            { 1, new Sword() },
            { 2, new Bow() },
            { 3, new Axe() },
            { 4, new HealthPotion() },
        };
    }
}
