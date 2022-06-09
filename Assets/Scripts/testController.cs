using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class testController : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public testCustomData customData;

    [SerializeField] private Text displayText;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("testRoom", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        customData = new testCustomData();
        customData.b = "dacsjoi";
    }

    public void CallRpc()
    {
        object[] data = { customData };
        if (PV.IsMine)
        {
            PV.RPC("rpc", RpcTarget.AllBuffered, data);
        }
    }

    [PunRPC]
    void rpc(object[] data)
    {
        customData = (testCustomData)data[0];
        displayText.text = customData.b;
    }
}
