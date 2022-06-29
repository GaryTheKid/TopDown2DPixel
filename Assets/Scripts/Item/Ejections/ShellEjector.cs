using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellEjector : MonoBehaviour
{
    [SerializeField] private Transform _shellPref;
    [SerializeField] private Transform _ejectionTrans;

    public void Eject()
    {
        Instantiate(_shellPref, _ejectionTrans.position, _ejectionTrans.rotation, GameManager.gameManager.spawnedProjectileParent);
    }
}
