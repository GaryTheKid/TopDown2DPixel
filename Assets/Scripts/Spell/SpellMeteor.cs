using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpellMeteor : MonoBehaviour
{
    public short spellID;
    public float explosiveRadius;
    public float explosiveSize_x;
    public float explosiveSize_y;
    public PhotonView attackerPV;

    [SerializeField] private AudioSource flyingSFX;
    [SerializeField] private AudioSource explosionSFX;

    private void OnEnable()
    {
        SFXManager.singleton.Add(flyingSFX);
        SFXManager.singleton.Add(explosionSFX);
    }

    private void OnDestroy()
    {
        SFXManager.singleton.Remove(flyingSFX);
        SFXManager.singleton.Remove(explosionSFX);
    }
}
