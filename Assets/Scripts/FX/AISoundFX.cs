using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISoundFX : MonoBehaviour
{
    [SerializeField] private AudioClip _BeingDamaged;

    private AudioSource _player;

    private void Awake()
    {
        _player = GetComponent<AudioSource>();
    }

    public void BeingDamaged()
    {
        _player.PlayOneShot(_BeingDamaged);
    }
}
