/* Last Edition: 06/11/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The target list for player melee weapon target locking.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TargetList : MonoBehaviour
{
    public HashSet<int> targets;

    [SerializeField] private SpriteRenderer _visual;
    private PhotonView _PV;

    private void Awake()
    {
        targets = new HashSet<int>();
        _PV = GetComponent<PhotonView>();
    }

    public void AddTarget(int targetID)
    {
        NetworkCalls.Player_NetWork.LockTarget(_PV, targetID);

        // show visual
        if (targets.Count > 0)
        {
            _visual.color = Color.red;
        }
    }

    public void RemoveTarget(int targetID)
    {
        NetworkCalls.Player_NetWork.UnlockTarget(_PV, targetID);

        // hide visual
        if (targets.Count <= 0)
        {
            _visual.color = Color.white;
        }
    }
}
