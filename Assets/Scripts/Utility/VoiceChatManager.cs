using UnityEngine;
using Photon.Pun;

public class VoiceChatManager : MonoBehaviour
{
    public static VoiceChatManager singleton;


    private void Awake()
    {
        singleton = this;
        //_PVC.SpeakerLinked += DetachNonSelfVoiceStream;
    }

    /*public void DetachNonSelfVoiceStream(Speaker speaker)
    {
        print(PhotonNetwork.LocalPlayer.ActorNumber + " " + speaker.RemoteVoice.PlayerId);


        if (speaker.RemoteVoice.PlayerId != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            speaker.RemoteVoice.
        }
    }*/
}
