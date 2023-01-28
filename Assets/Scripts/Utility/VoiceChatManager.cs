using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using Photon.Voice;

public class VoiceChatManager : MonoBehaviour
{
    public static VoiceChatManager singleton;

    [SerializeField] private PunVoiceClient _PVC;
    [SerializeField] private Recorder _recorder;

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
