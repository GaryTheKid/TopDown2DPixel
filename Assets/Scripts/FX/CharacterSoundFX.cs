using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFX : MonoBehaviour
{
    [SerializeField] private AudioClip _consumePotion;
    [SerializeField] private AudioClip _speedBoost;
    [SerializeField] private AudioClip _beingDamaged;
    [SerializeField] private AudioClip _killFeedBack;
    [SerializeField] private AudioClip _blink;

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
        _player.PlayOneShot(_speedBoost);
    }

    public void BeingDamaged()
    {
        _player.PlayOneShot(_beingDamaged);
    }

    public void KillFeedBack()
    {
        _player.PlayOneShot(_killFeedBack);
    }

    public void Blink()
    {
        _player.PlayOneShot(_blink);
    }
}
