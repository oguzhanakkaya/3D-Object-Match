using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameEvents;

public class LevelFailedUI : MonoBehaviour
{
    private EventBus _eventBus;

    public Button quitButton, resumeButton;

    public void Initialize()
    {
        _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
        _eventBus.Subscribe<GameEvents.OnLevelFailed>(OnLevelFailed);

        quitButton.onClick.AddListener(QuitLevel);
        resumeButton.onClick.AddListener(RetryLevel);
    }
    private void OnLevelFailed()
    {
        gameObject.SetActive(true);
    }

    private void RetryLevel()
    {
        gameObject.SetActive(false);
        LevelManager.Instance.LoadNextLevel();
        GameUI.Instance.StartTimer();
    }
    private void QuitLevel()
    {
        gameObject.SetActive(false);
        GameUI.Instance.BackToLobby();
    }
}
