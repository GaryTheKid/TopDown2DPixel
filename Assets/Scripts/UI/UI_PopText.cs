using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_PopText : MonoBehaviour
{
    public enum TextType
    {
        Damage,
        Heal
    }

    public TextType textType;
    public int textAmount;

    [SerializeField] private Color damageTextColor;
    [SerializeField] private Color HealTextColor;
    [SerializeField] private float activationTime;
    [SerializeField] private float fadingTime;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        switch (textType)
        {
            case TextType.Damage:
                GetComponent<TextMeshPro>().color = damageTextColor;
                break;
            case TextType.Heal:
                GetComponent<TextMeshPro>().color = HealTextColor;
                break;
        }

        GetComponent<TextMeshPro>().text = textAmount.ToString();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
