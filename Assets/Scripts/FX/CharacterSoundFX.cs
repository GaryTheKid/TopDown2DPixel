using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFX : MonoBehaviour
{
    [SerializeField] private AudioClip _consumePotion;
    private AudioSource _player;

    private void Awake()
    {
        _player = GetComponent<AudioSource>();
    }

    public void ConsumePotion()
    {
        _player.PlayOneShot(_consumePotion);
    }
}
