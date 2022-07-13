using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Observable_AITransform : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 pos;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(rb.velocity);
        }
        else
        {
            pos = (Vector3)stream.ReceiveNext();
            rb.velocity = (Vector2)stream.ReceiveNext();
            //rigidbody2D.position = (Vector2)stream.ReceiveNext();
            transform.position = Vector3.Lerp(transform.position, pos, 0.1f);

            // lag compensation
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            rb.position += rb.velocity * lag;
        }
    }
}
