using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitFX : MonoBehaviour
{
    [SerializeField] private GameObject _bulletParent;

    public void DestroySelf()
    {
        Destroy(_bulletParent);
    }
}
