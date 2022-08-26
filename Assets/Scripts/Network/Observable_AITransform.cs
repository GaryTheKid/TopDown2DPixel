using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Observable_AITransform : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 pos;
    private bool isReceived;

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
            return;

        if (isReceived && transform.position != pos)
        {
            transform.position += (pos - transform.position) * 0.2f;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            pos = (Vector3)stream.ReceiveNext();
            isReceived = true;
        }   
    }
}
