using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SocialWheelChoice : MonoBehaviour
{
    [SerializeField] private GameObject selectionHighlight;

    public void ShowHighlight()
    {
        selectionHighlight.SetActive(true);
    }

    public void HideHighlight()
    {
        selectionHighlight.SetActive(false);
    }
}
