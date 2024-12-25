using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float delaySpawnTime;
    public CharacterColor color;
    public PlayerController player;
    private float timer;
    private bool isSpawned;
    private GameObject monster;

    void Start()
    {
        timer = 0;

        monster = Instantiate(monsterPrefab, transform);
        var monsterCtrl = monster.GetComponent<MonsterController>();
        monsterCtrl.target = player;
        monsterCtrl.spriteRenderer.color = ColorData.GetColor(color);
        monster.GetComponent<MonsterController>().bulletColor = color;
        monster.SetActive(false);
    }

    void Update()
    {
        SpawnMonster();
    }

    void SpawnMonster()
    {
        if (isSpawned) return;

        timer += Time.deltaTime;
        if (timer > delaySpawnTime)
        {
            monster.SetActive(true);
            isSpawned = true;
        }
    }
}
