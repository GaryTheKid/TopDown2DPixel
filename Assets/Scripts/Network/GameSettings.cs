using UnityEngine;

public class GameSettings : MonoBehaviour
{
    // singleton
    public static GameSettings gameSettings;

    // fields
    public string gameVersion;
    public string playerName;
    public int gameSceneIndex;
    
    private void Awake()
    {
        gameSettings = this;
    }
}
