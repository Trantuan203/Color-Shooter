using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveBarUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject heartPrefab;
    private int StartHealth;
    private List<GameObject> heartList;
    void Start()
    {
        StartHealth = player.health;
        InstantiateLiveBar();
    }

    private void Awake()
    {
        GameEvent.OnPlayerIsShot += DecreaseLiveBar;
    }

    private void OnDestroy()
    {
        GameEvent.OnPlayerIsShot -= DecreaseLiveBar;
    }

    private void InstantiateLiveBar()
    {
        heartList = new();

        for(int i = 0; i < StartHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            heartList.Add(heart);
        }
    }

    private void DecreaseLiveBar(int health)
    {
        if(health >= 0)
            heartList[health].SetActive(false);
    }
}
