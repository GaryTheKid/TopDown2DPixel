using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void RevealBush()
    {
        _animator.SetTrigger("Reveal");
    }

    public void HideBush()
    {
        _animator.SetTrigger("Hide");
    }
}
