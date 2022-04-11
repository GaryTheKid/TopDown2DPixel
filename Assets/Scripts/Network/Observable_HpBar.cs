using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Observable_HpBar : MonoBehaviour
{
    private Image hpBar;

    private void Awake()
    {
        hpBar = GetComponent<Image>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hpBar.fillAmount);
        }
        else
        {
            hpBar.fillAmount = (float)stream.ReceiveNext();
        }
    }
}
