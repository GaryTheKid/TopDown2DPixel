using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    // singleton
    public static GameSettings gameSettings;

    // fields
    public string gameVersion;
    public string playerName;
    public int gameSceneIndex;
    public int sendRate;
    public int serializationRate;

    [SerializeField] private Text nameTextUI;

    private void Awake()
    {
        gameSettings = this;
    }

    public void SetPlayerName()
    {
        playerName = nameTextUI.text;
    }
}
