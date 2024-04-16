using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameEvents;

public enum GameState
{
    Loading,
    Lobby,
    Transition,
    Game
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public EventBus _eventBus;

    private GameState currentState;

    public InputManager inputManager;
    public LevelManager levelManager;
    public UIManager uiManager;
    public TileParent tileParent;

    public int coin;
    private int goalCount;
    public int level;

    private void Awake()
    {
        Instance = this;

        Container.Initialize();

        level = PlayerPrefs.GetInt("Level", 0);

        InitiliazeManagers();

        _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
        _eventBus.Subscribe<GameEvents.OnStateChanged>(OnStateChanged);
        _eventBus.Subscribe<GameEvents.OnGoalCompleted>(OnGoalCompleted);
        _eventBus.Subscribe<GameEvents.OnLevelCompleted>(OnLevelCompleted);


        _eventBus.Fire(new GameEvents.OnStateChanged(GameState.Loading));

        SetCoin();
    }
    private void OnStateChanged(GameEvents.OnStateChanged gameEvent)
    {
        currentState = gameEvent.state;
    }
    public GameState GetCurrentStage()
    {
        return currentState;
    }
    private void SetCoin()
    {
        coin = PlayerPrefs.GetInt("Coin",0);

        _eventBus.Fire(new GameEvents.OnCoinChanged(coin));
    }
    private void InitiliazeManagers()
    {
        uiManager.Initialize();
        inputManager.Initialize();
        levelManager.Initialize();
        tileParent.Initialize();
    }
    public void SetGoalCount(int goalCount)
    {
        this.goalCount = goalCount;
    }
    private void OnGoalCompleted()
    {
        goalCount--;

        if (goalCount<=0)
        {
            _eventBus.Fire(new GameEvents.OnLevelEnded());
            _eventBus.Fire(new GameEvents.OnLevelCompleted());
        }
    }
    private void OnLevelCompleted()
    {
        PlayerPrefs.SetInt("Level", level += 1);
    }
}
