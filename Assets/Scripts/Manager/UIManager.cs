using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject gameOverUI;
    
    protected override void Awake()
    {
        GameEvent.OnPlayerDead += ShowGameOverUI;
    }

    private void OnDestroy()
    {
        GameEvent.OnPlayerDead -= ShowGameOverUI;
    }

    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }


    public void HideGameOverUI()
    {
        gameOverUI.SetActive(false);
    }

    public void ShowUI()
    {

    }

    public void HideUI()
    {

    }
}
