using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameEvents;
using DG.Tweening;

public class LoadingUI : MonoBehaviour
{
    public EventBus _eventBus;

    [SerializeField]private Slider slider;

    private float sliderValue;

    


    public void Initialize()
    {
        _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
        _eventBus.Subscribe<GameEvents.OnStateChanged>(OnStateChanged);

    }
    private void OnStateChanged(GameEvents.OnStateChanged gameEvents)
    {
        if (gameEvents.state==GameState.Loading || gameEvents.state == GameState.Transition)
        {
            gameObject.SetActive(true);
            FillBar();
        }
    }
    private void FillBar()
    {
        sliderValue = .1f;

        DOTween.To(()=> sliderValue,x=> sliderValue = x, 1f, 3f).SetEase(Ease.Linear)
            .OnUpdate(() =>
        {
            slider.value = sliderValue;
        })
            .OnComplete(() =>
        {
            SliderFilled();
        });
    }
    private void SliderFilled()
    {
        gameObject.SetActive(false);

        if (GameManager.Instance.GetCurrentStage()==GameState.Loading)
            _eventBus.Fire(new GameEvents.OnStateChanged(GameState.Lobby));
        else
            _eventBus.Fire(new GameEvents.OnStateChanged(GameState.Game));

    }
}
