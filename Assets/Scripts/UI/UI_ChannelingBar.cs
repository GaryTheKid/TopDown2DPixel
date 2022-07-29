using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChannelingBar : MonoBehaviour
{
    [SerializeField] private Image _progressBar;

    public void Activate()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void Deactivate()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void UpdateProgress(float progress)
    {
        _progressBar.fillAmount = progress;
    }
}
