using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelDataSO : ScriptableObject
{
    public List<Level> LevelMission = new();

    public List<EnemyData> GetEnemyDataByLevel(int level)
    {
        return LevelMission.Find(item => item.level == level).enemyData;
    }
}

[Serializable]
public class Level
{
    public int level;
    public List<EnemyData> enemyData = new();
}

[Serializable]
public class EnemyData
{
    public CharacterColor characterColor;
    public int quantity;
}
