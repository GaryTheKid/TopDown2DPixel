using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFX : MonoBehaviour
{
    [SerializeField] private AudioClip _consumePotion;
    [SerializeField] private AudioClip _SpeedBoost;
    [SerializeField] private AudioClip _BeingDamaged;

    private AudioSource _player;

    private void Awake()
    {
        _player = GetComponent<AudioSource>();
    }

    public void ConsumePotion()
    {
        _player.PlayOneShot(_consumePotion);
    }

    public void SpeedBoost()
    {
        _player.PlayOneShot(_SpeedBoost);
    }

    public void BeingDamaged()
    {
        _player.PlayOneShot(_BeingDamaged);
    }
}
