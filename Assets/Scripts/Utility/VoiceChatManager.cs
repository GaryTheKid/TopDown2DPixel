using UnityEngine;
using Photon.Voice.PUN;
using Photon.Voice.Unity;

public class VoiceChatManager : MonoBehaviour
{
    public static VoiceChatManager singleton;

    [SerializeField] private PunVoiceClient _PVC;
    [SerializeField] private Recorder _recorder;

    private void Awake()
    {
        singleton = this;
    }

    public void AddSpeaker(Speaker speaker)
    {
        _PVC.AddSpeaker(speaker, null);
    }
}
