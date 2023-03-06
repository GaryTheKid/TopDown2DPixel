using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PageSwitch : MonoBehaviour
{
    public Animator pageAnimator;
    public float pagePos;

    // Update is called once per frame
    void Update()
    {
        pageAnimator.SetFloat("PagePos", pagePos);
    }
}
