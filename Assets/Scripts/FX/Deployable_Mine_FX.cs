using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deployable_Mine_FX : DeployableFX
{
    [SerializeField] private GameObject _explosionTrigger;

    public override void FireFX()
    {
        _explosionTrigger.SetActive(true);
    }
}
