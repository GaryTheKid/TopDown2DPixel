using UnityEngine;

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

    private void Awake()
    {
        gameSettings = this;
    }
}
