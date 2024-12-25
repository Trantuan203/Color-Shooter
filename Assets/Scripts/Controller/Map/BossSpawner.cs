using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public float delaySpawnTime;
    public PlayerController player;
    private float timer;
    private bool isSpawned;
    private GameObject boss;
    public CharacterColor color;
    BossController bossCtrl;

    void Start()
    {
        timer = 0;

        boss = Instantiate(bossPrefab, transform);
        bossCtrl = boss.GetComponent<BossController>();
        bossCtrl.target = player;

        SetBossRandomColor();

        boss.SetActive(false);

        InvokeRepeating("SetBossRandomColor", 10f, 10f);
    }

    void Update()
    {
        SpawnBoss();
    }

    void SpawnBoss()
    {
        if (isSpawned) return;

        timer += Time.deltaTime;
        if (timer > delaySpawnTime)
        {
            boss.SetActive(true);
            isSpawned = true;
        }
    }

    void SetBossRandomColor()
    {
        color = ColorData.GetRandomColor();

        bossCtrl.spriteRenderer.color = ColorData.GetColor(color);
        bossCtrl.BossColor = color;

    }
}
