using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Observable_WeaponTransform : MonoBehaviourPunCallbacks, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.eulerAngles.z);
        }
        else
        {
            float zDeg = (float)stream.ReceiveNext();
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, 0f, zDeg), 0.01f);
        }
    }
}
