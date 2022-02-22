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

    // prefab for item world
    [Header("ItemWorld Prefab")]
    public Transform pfItemWorld;

    // Sprite for inventory and itemworld
    [Header("Sprites")]
    public Sprite swordSprite;
    public Sprite bowSprite;
    public Sprite healthPotionSprite;
}
