using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Text;
using TMPro;
using UnityEngine;
using static GameEvents;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public EventBus _eventBus;

    public static GameUI Instance;

    public List<GoalUI> goalsUIList = new List<GoalUI>();

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timeText;

    private int time;
    private Coroutine timerCoroutine;
    private StringBuilder stringBuilder = new StringBuilder();

    [SerializeField]private Button backToLobbyButton;

    public void Initialize()
    {
        if (Instance == null)
            Instance = this;

        _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
        _eventBus.Subscribe<GameEvents.OnStateChanged>(OnStateChanged);
        _eventBus.Subscribe<GameEvents.OnLevelLoaded>(OnLevelLoaded);
        _eventBus.Subscribe<GameEvents.OnLevelEnded>(StopTimer);

        backToLobbyButton.onClick.AddListener(BackToLobby);
    }
    private void OnStateChanged(GameEvents.OnStateChanged gameEvents)
    {
        if (gameEvents.state == GameState.Game)
        {
            gameObject.SetActive(true);
            StartTimer();
        }
    }
    private void OnLevelLoaded(GameEvents.OnLevelLoaded gameEvents)
    {
        SetLevelText(gameEvents.level);
        SetGoals(gameEvents.levelData);
        SetTime(gameEvents.levelData.time);

        GameManager.Instance.SetGoalCount(gameEvents.levelData.goals.Count);
    }
    private void SetGoals(LevelData levelData)
    {
        foreach (var item in goalsUIList)
            item.gameObject.SetActive(false);

        for (int i = 0; i < levelData.goals.Count; i++)
        {
            goalsUIList[i].Initialize();
            goalsUIList[i].SetGoalUI(levelData.goals[i].item, levelData.goals[i].goalNumber);
        }
    }
    private void SetLevelText(int level)
    {
        levelText.text = "Level " + (level+1).ToString();
    }
    private void SetTime(int time)
    {
        this.time = time;
    }
    public void StartTimer()
    {
        if (timerCoroutine != null)
            return;

        timerCoroutine = StartCoroutine(TimerCoroutine());
    }
    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }
    public IEnumerator TimerCoroutine()
    {
        SetTimeText(time);

        while (true)
        {
            yield return new WaitForSeconds(1f);
            --time;

            SetTimeText(time);

            if (time == 0)
            {
                StopTimer();
                _eventBus.Fire(new GameEvents.OnLevelFailed());
                _eventBus.Fire(new GameEvents.OnLevelEnded());

                break;
            }
        }
    }
    public void SetTimeText(int time)
    {
        stringBuilder.Clear();
        stringBuilder.Append(string.Format("{0:00}:{1:00}", TimeSpan.FromSeconds(time).Minutes, TimeSpan.FromSeconds(time).Seconds));

        timeText.text = stringBuilder.ToString(); ;


        if (time <= 5)
        {
            timeText.transform.DOScale(1.5f, .25f).SetLoops(2, LoopType.Yoyo);
            timeText.DOColor(Color.red, .25f).SetLoops(2, LoopType.Yoyo);
        }
    }
    public void ResetTimeText()
    {
        DOTween.Kill(timeText);
        DOTween.Kill(timeText.transform);

        timeText.transform.localScale = Vector3.one;
        timeText.DOColor(Color.white, 0);
    }
    public void BackToLobby()
    {
        gameObject.SetActive(false);
        _eventBus.Fire(new GameEvents.OnLevelEnded());
        _eventBus.Fire(new GameEvents.OnStateChanged(GameState.Loading));

    }
}
