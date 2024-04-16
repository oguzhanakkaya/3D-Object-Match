using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using static GameEvents;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public EventBus _eventBus;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField]private Button playButton;

    public void Initialize()
    {
        _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
        _eventBus.Subscribe<GameEvents.OnStateChanged>(OnStateChanged);
        _eventBus.Subscribe<GameEvents.OnCoinChanged>(OnCoinChanged);


        playButton.onClick.AddListener(PlayButtonPressed);

    }
    private void OnStateChanged(GameEvents.OnStateChanged gameEvents)
    {
        if (gameEvents.state == GameState.Lobby)
            gameObject.SetActive(true);

        levelText.text = (GameManager.Instance.level + 1).ToString();
    }
    private void OnCoinChanged(GameEvents.OnCoinChanged gameEvents)
    {
    }
    private void PlayButtonPressed()
    {
        gameObject.SetActive(false);
        _eventBus.Fire(new GameEvents.OnStateChanged(GameState.Transition));
        _eventBus.Fire(new GameEvents.OnLevelStarted());
    }
}
