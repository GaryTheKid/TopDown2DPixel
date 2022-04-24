using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets itemAssets;

    private void Awake()
    {
        itemAssets = this;
    }

    // prefab for weapon Equipped
    [Header("Equipment Prefabs")]
    public Transform pfSword;
    public Transform pfAxe;
    public Transform pfBow;
    public Transform pfGun;

    // prefab for weapon Equipped
    [Header("Projectile Prefabs")]
    public Transform pfArrow;

    // prefab for item world
    [Header("ItemWorld Prefab")]
    public Transform pfItemWorld;

    // Sprite for inventory and itemworld
    [Header("Sprites")]
    public Sprite swordSprite;
    public Sprite axeSprite;
    public Sprite bowSprite;
    public Sprite gunSprite;

    public Sprite arrowSprite;

    public Sprite healthPotionSprite;
}
