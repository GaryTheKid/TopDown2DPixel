using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Observable_Transform : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 pos;
    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(rigidbody2D.velocity);
            //stream.SendNext(rigidbody2D.position);
        }
        else
        {
            pos = (Vector3)stream.ReceiveNext();
            rigidbody2D.velocity = (Vector2)stream.ReceiveNext();
            //rigidbody2D.position = (Vector2)stream.ReceiveNext();
            transform.position = Vector3.Lerp(transform.position, pos, 0.1f);
        }
    }
}
