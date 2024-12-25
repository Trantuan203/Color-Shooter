using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameController : Singleton<IngameController>
{
    [Header("---------- Audio ----------")]
    public AudioClip musicIngame;
    public AudioClip clickButtonClip;
    public AudioClip displayGateClip;
    public AudioClip displayDoneObjectiveClip;
    public AudioClip defeatBossClip;

    [Header("---------- Button ----------")]
    public Button btnPause;
    public Button btnResume;
    public Button[] btnsReplay;
    public Button btnSound;
    public Button btnMusic;
    public Button btnHome;
    public Button btnHomeOver;
    public Button btnHomeWin;

    [Header("---------- Panel ----------")]
    public GameObject panelPause;
    public GameObject panelBoss;
    public Slider barBossHP;
    public GameObject panelWinBoss;

    [Header("---------- Text ----------")]
    public Text levelText;

    private bool soundState;
    private bool musicState;

    [SerializeField] Sprite checkBg;
    [SerializeField] Sprite unCheckBg;

    protected override void Awake()
    {
        base.Awake();
        GameEvent.OnDisplayStartGate += PlayGateSound;
        GameEvent.OnCompleteLevel += PlayWinSound;
        GameEvent.OnBossKilled += DisplayWinGame;
    }

    void Start()
    {
        AudioManager.instance.musicSource.clip = musicIngame;
        AudioManager.instance.musicSource.Play();

        panelPause.SetActive(false);
        panelBoss.SetActive(false);
        panelWinBoss.SetActive(false);
        barBossHP.wholeNumbers = true;

        soundState = GameManager.instance.GetSoundState();
        musicState = GameManager.instance.GetMusicState();

        btnSound.GetComponent<Image>().sprite = soundState ? checkBg : unCheckBg;
        btnMusic.GetComponent<Image>().sprite = musicState ? checkBg : unCheckBg;

        RemoveButtonListener(btnsReplay);
        RemoveButtonListener(btnPause, btnResume, btnSound, btnMusic, btnHome);

        foreach (var btn in btnsReplay)
        {
            btn.onClick.AddListener(Replay);
        }
        btnPause.onClick.AddListener(Pause);
        btnResume.onClick.AddListener(Resume);
        btnSound.onClick.AddListener(ToggleSound);
        btnMusic.onClick.AddListener(ToggleMusic);
        btnHome.onClick.AddListener(GoHome);
        btnHomeOver.onClick.AddListener(GoHome);
        btnHomeWin.onClick.AddListener(GoHome);
    }

    private void OnDestroy()
    {
        RemoveButtonListener(btnsReplay);
        RemoveButtonListener(btnPause, btnResume, btnSound, btnMusic, btnHome, btnHomeOver, btnHomeWin);

        GameEvent.OnDisplayStartGate -= PlayGateSound;
        GameEvent.OnCompleteLevel -= PlayWinSound;
        GameEvent.OnBossKilled -= DisplayWinGame;
    }

    private void RemoveButtonListener(params Button[] buttons)
    {
        foreach (var button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    public void SetLevelText(int level)
    {
        levelText.text = "LEVEL " + level;
    }

    public void Pause()
    {
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        Time.timeScale = 0;
        panelPause.gameObject.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        panelPause.gameObject.SetActive(false);
    }

    public void Replay()
    {
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void GoHome()
    {
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        SceneManager.LoadScene("Home");
        Time.timeScale = 1;
    }

    public void DisplayWinGame()
    {
        StartCoroutine(OpenWinPanel());
    }

    IEnumerator OpenWinPanel()
    {
        yield return new WaitForSeconds(3.0f);
        Time.timeScale = 0;
        panelWinBoss.gameObject.SetActive(true);
    }

    void ToggleSound()
    {
        soundState = !soundState;
        AudioManager.instance.soundSource.mute = !soundState;
        GameManager.instance.SetSoundState(soundState);
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        btnSound.GetComponent<Image>().sprite = soundState ? checkBg : unCheckBg;
    }
    
    void ToggleMusic()
    {
        musicState = !musicState;
        AudioManager.instance.musicSource.mute = !musicState;
        GameManager.instance.SetMusicState(musicState);
        AudioManager.instance.soundSource.PlayOneShot(clickButtonClip);
        btnMusic.GetComponent<Image>().sprite = musicState ? checkBg : unCheckBg;
    }

    public void SetBossHP(float curHP, float maxHP)
    {
        barBossHP.minValue = 0;
        barBossHP.maxValue = maxHP;
        barBossHP.value = curHP;
    }

    public void PlayGateSound()
    {
        AudioManager.instance.soundSource.PlayOneShot(displayGateClip);
    }

    public void PlayBossDefeatedSound()
    {
        AudioManager.instance.soundSource.PlayOneShot(defeatBossClip);
    }

    public void PlayWinSound()
    {
        AudioManager.instance.soundSource.PlayOneShot(displayDoneObjectiveClip);
    }
}
