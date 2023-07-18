using Photon.Pun;
using UnityEngine;

public class HolySacrificeFX : MonoBehaviour
{
    public bool isEnabled;

    [SerializeField] private AudioSource _sfx;
    [SerializeField] private GameObject _fxTrigger;

    private HolySacrificeTrigger _trigger;

    private void Awake()
    {
        _trigger = _fxTrigger.GetComponent<HolySacrificeTrigger>();
    }

    public void SetFXInfo(PhotonView PV, DamageInfo dmgInfo)
    {
        _trigger.SetTrigger(PV, dmgInfo);
        SFXManager.singleton.Add(_sfx);
    }

    public void Activate()
    {
        _fxTrigger.SetActive(true);
        _sfx.PlayOneShot(_sfx.clip);
    }
}
