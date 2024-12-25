using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button replayBtn;

    private void Awake()
    {
        replayBtn.onClick.AddListener(HandleReplayGame);
    }

    private void OnDestroy()
    {
        replayBtn.onClick.RemoveAllListeners();
    }

    private void HandleReplayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneController.Instance.ReLoadScene();

        Time.timeScale = 1;
    }
}
