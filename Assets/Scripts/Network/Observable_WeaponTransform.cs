using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Observable_WeaponTransform : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 rotation;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.eulerAngles);
        }
        else
        {
            rotation = (Vector3)stream.ReceiveNext();
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, rotation, 0.1f);
        }
    }
}
