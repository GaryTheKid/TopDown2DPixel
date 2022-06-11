using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class targetDetector : MonoBehaviour
{
    private TargetList _targetList;

    private void Awake()
    {
        _targetList = transform.GetComponentInParent<TargetList>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        if (target != null && collision.gameObject.CompareTag("EnemyPlayer"))
        {
            _targetList.AddTarget(target.transform.parent.GetComponent<PhotonView>().ViewID);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        if (target != null && collision.gameObject.CompareTag("EnemyPlayer"))
        {
            _targetList.RemoveTarget(target.transform.parent.GetComponent<PhotonView>().ViewID);
        }
    }
}
