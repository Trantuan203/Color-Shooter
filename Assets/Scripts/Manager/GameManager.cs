using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void MakeSingleInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    void Awake()
    {
        MakeSingleInstance();
        IsFirstTimePlay();
    }

    private const string FIRST_TIME_PLAY = "IsFirstTimePlay";
    private const string PROGRESS = "Progress";
    private const string SOUND = "Sound";
    private const string MUSIC = "Music";

    void IsFirstTimePlay()
    {
        if (!PlayerPrefs.HasKey(FIRST_TIME_PLAY))
        {
            PlayerPrefs.SetInt(FIRST_TIME_PLAY, 0);
            PlayerPrefs.SetInt(PROGRESS, 1);
            PlayerPrefs.SetInt(SOUND, 1);
            PlayerPrefs.SetInt(MUSIC, 1);
            PlayerPrefs.Save();
        }
    }

    public ColorData colorData;
    public bool introDisplayed;

    void Start()
    {
        introDisplayed = false;
    }

    public void SetLevel(int level)
    {
        PlayerPrefs.SetInt(PROGRESS, level);
        PlayerPrefs.Save();
    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt(PROGRESS);
    }

    public void SetSoundState(bool isTurnedOn)
    {
        PlayerPrefs.SetInt(SOUND, isTurnedOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool GetSoundState()
    {
        return PlayerPrefs.GetInt(SOUND) == 1;
    }

    public void SetMusicState(bool isTurnedOn)
    {
        PlayerPrefs.SetInt(MUSIC, isTurnedOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool GetMusicState()
    {
        return PlayerPrefs.GetInt(MUSIC) == 1;
    }

    public void ResetGameData()
    {
        PlayerPrefs.DeleteKey(PROGRESS);
        PlayerPrefs.DeleteKey(FIRST_TIME_PLAY);
        PlayerPrefs.DeleteKey(SOUND);
        PlayerPrefs.DeleteKey(MUSIC);
        PlayerPrefs.Save();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
    Application.Quit();
#elif UNITY_WEBGL
    Application.ExternalEval("document.location.reload(true);");
#endif
    }

}
