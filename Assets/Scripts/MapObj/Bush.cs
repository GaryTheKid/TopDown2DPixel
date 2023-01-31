using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public bool isCharacterInside;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void RevealBush()
    {
        _animator.SetBool("Reveal", true);
    }

    public void HideBush()
    {
        if (isCharacterInside)
            return;

        _animator.SetBool("Reveal", false);
    }
}
