using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFogMask : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetFogOn()
    {
        if (animator.GetBool("isFogOn"))
            return;

        animator.SetBool("isFogOn", true);
    }

    public void SetFogOff()
    {
        if (!animator.GetBool("isFogOn"))
            return;

        animator.SetBool("isFogOn", false);
    }
}
