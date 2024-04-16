using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEvents;
using Lean.Pool;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    private EventBus _eventBus;

    public List<LevelData> levelsList = new List<LevelData>();

    public Transform levelParent;

    private List<float> borderAmounts = new List<float>();
   

    public void Initialize()
    {
        _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
        _eventBus.Subscribe<GameEvents.OnLevelStarted>(LoadNextLevel);

        Instance = this;
    }
    public void LoadNextLevel()
    {
        ClearLevel();

        LevelData currentLevelData = ParseLevelRemote(GameManager.Instance.level);
        foreach (var item in currentLevelData.levelObjects)
        {
            for (int i = 0; i < item.itemNumber; i++)
            {
                var obj=LeanPool.Spawn(ItemDataManager.GetObjectFromEnum(item.item));
                obj.transform.position = RandomPointInBounds();
                obj.transform.SetParent(levelParent);
            }
        }

        _eventBus.Fire(new GameEvents.OnLevelLoaded(GameManager.Instance.level, currentLevelData));
    }
    private LevelData ParseLevelRemote(int levelNo)
    {
        if (levelNo >= levelsList.Count)
        {
            return levelsList[levelNo%levelsList.Count];
        }
        else
            return levelsList[levelNo];
    }
    private void ClearLevel()
    {
        LeanPool.DespawnAll();
    }
    public void SetBorderList(List<float> floats)
    {
        borderAmounts = floats;
    }
    
    private Vector3 RandomPointInBounds()
    {
        return new Vector3(
           UnityEngine.Random.Range(borderAmounts[3], borderAmounts[2]),
           UnityEngine.Random.Range(0.5f, 3.5f),
           UnityEngine.Random.Range(borderAmounts[1], borderAmounts[0]));
    }
}
