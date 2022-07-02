using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Scene : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ShowSceneName();
    }

    public void ShowSceneName()
    {
        animator.SetTrigger("ShowSceneName");
    }
}
