using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameEvents;

public class LevelWinUI : MonoBehaviour
{
    private EventBus _eventBus;

    public Button resumeButton;

    public void Initialize()
    {
        _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
        _eventBus.Subscribe<GameEvents.OnLevelCompleted>(OnLevelCompleted);

        resumeButton.onClick.AddListener(ResumeButtonClicked);
    }
    private void OnLevelCompleted()
    {
        gameObject.SetActive(true);
    }
    private void ResumeButtonClicked()
    {
        gameObject.SetActive(false);
        GameUI.Instance.BackToLobby();
    }
}
