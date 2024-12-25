using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private LevelDataSO levelData;
    [SerializeField] private DesignDataSO designData;
    private int currentLevel;
    private List<EnemyData> currentEnemyData;

    private void Start()
    {
        currentLevel = SceneController.Instance.GetCurrentSceneIndex();
        currentEnemyData = levelData.GetEnemyDataByLevel(currentLevel);

    }

    
}
