using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : Singleton<HomeController>
{
    private List<GameObject> panelObjList = new List<GameObject>();

    [Header("---------- Audio ----------")]
    public AudioClip musicHome;
    public AudioClip clickButtonClip;
    public AudioClip gameStartClip;

    [Header("---------- Button ----------")]
    public Button btnPlay;
    public Button btnSetting;
    public Button btnSoundOff;
    public Button btnSoundOn;
    public Button btnMusicOff;
    public Button btnMusicOn;
    public Button btnCredits;
    public Button btnTutorial;
    public Button btnResetData;
    public Button btnConfirmResetData;
    public Button[] btnsBackToMenu;
    public Button[] btnsLevel;

    [Header("---------- Panel ----------")]
    public GameObject panelMenu;
    public GameObject panelMap;

    [Header("---------- Tab ----------")]
    public GameObject tabSetting;
    public GameObject tabCredits;
    public GameObject tabTutorial;
    public GameObject tabResetData;

    void Start()
    {
        panelObjList.Add(panelMenu);
        panelObjList.Add(panelMap);

        ShowPanel(panelMenu);

        AudioManager.instance.soundSource.mute = !GameManager.instance.GetSoundState();
        AudioManager.instance.musicSource.mute = !GameManager.instance.GetMusicState();
        AudioManager.instance.musicSource.clip = musicHome;
        AudioManager.instance.musicSource.Play();
        btnSoundOff.gameObject.SetActive(AudioManager.instance.soundSource.mute);
        btnMusicOff.gameObject.SetActive(AudioManager.instance.musicSource.mute);
        tabSetting.SetActive(false);
        tabCredits.SetActive(false);
        tabTutorial.SetActive(false);
        tabResetData.SetActive(false);

        HandleLevelButtons();
        RemoveButtonListener(btnsBackToMenu);
        RemoveButtonListener(btnPlay, btnSetting, btnSoundOff, btnSoundOn, btnMusicOff, btnMusicOn, btnCredits, 
            btnTutorial, btnResetData, btnConfirmResetData);

        foreach (var backBt in btnsBackToMenu)
        {
            backBt.onClick.AddListener(BackToMenu);
        }
        btnPlay.onClick.AddListener(PlayGame);
        btnSetting.onClick.AddListener(Setting);
        btnSoundOff.onClick.AddListener(TurnOnSound);
        btnSoundOn.onClick.AddListener(TurnOffSound);
        btnMusicOff.onClick.AddListener(TurnOnMusic);
        btnMusicOn.onClick.AddListener(TurnOffMusic);
        btnCredits.onClick.AddListener(OpenCredits);
        btnTutorial.onClick.AddListener(OpenTutorial);
        btnResetData.onClick.AddListener(NotifyResetData);
        btnConfirmResetData.onClick.AddListener(ResetData);
    }

    public void ShowPanel(GameObject panelObj)
    {
        foreach (var obj in panelObjList)
        {
            if (obj == panelObj)
            {
                obj.SetActive(true);
            }
            else
            {
                if (obj.activeInHierarchy)
                {
                    obj.SetActive(false);
                }
            }
        }
    }

    private void RemoveButtonListener(params Button[] buttons)
    {
        foreach (var button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void HandleLevelButtons()
    {
        //RemoveButtonListener(btnsLevel);

        //for (int j = 0; j < btnsLevel.Length; j++)
        //{
        //    btnsLevel[j].onClick.AddListener(() =>
        //    {
        //        JoinLevel(j + 1);
        //    });
        //}

        for (int i = 0; i < btnsLevel.Length; i++)
        {
            if (i < GameManager.instance.GetLevel())
            {
                btnsLevel[i].interactable = true;
            }
            else
            {
                btnsLevel[i].interactable = false;
            }
        }
    }

    void PlayGame()
    {
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        ShowPanel(panelMap);
    }

    void Setting()
    {
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        tabSetting.SetActive(true);
    }

    void TurnOnSound()
    {
        AudioManager.instance.soundSource.mute = false;
        GameManager.instance.SetSoundState(true);
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        btnSoundOff.gameObject.SetActive(false);
    }

    void TurnOffSound()
    {
        AudioManager.instance.soundSource.mute = true;
        GameManager.instance.SetSoundState(false);
        btnSoundOff.gameObject.SetActive(true);
    }

    void TurnOnMusic()
    {
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        AudioManager.instance.musicSource.mute = false;
        GameManager.instance.SetMusicState(true);
        btnMusicOff.gameObject.SetActive(false);
    }

    void TurnOffMusic()
    {
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        AudioManager.instance.musicSource.mute = true;
        GameManager.instance.SetMusicState(false);
        btnMusicOff.gameObject.SetActive(true);
    }
    
    void OpenCredits()
    {
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        tabCredits.SetActive(true);
    }

    void OpenTutorial()
    {
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        tabTutorial.SetActive(true);
    }

    void NotifyResetData()
    {
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        tabResetData.SetActive(true);
    }

    void ResetData()
    {
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        GameManager.instance.ResetGameData();
    }

    void BackToMenu()
    {
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        ShowPanel(panelMenu);
    }

    public void JoinLevel(int level)
    {
        AudioManager.instance.soundSource.PlayOneShot(gameStartClip);
        SceneManager.LoadScene("Level " + level);
    }
}
