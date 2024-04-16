using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using static GameEvents;
using DG.Tweening;

public class GoalUI : MonoBehaviour
{
    private EventBus _eventBus;


    private ItemsEnum itemEnum;
    private int itemCount;

    public TextMeshProUGUI numberText;
    public Image iconImage;


    public void Initialize()
    {
        _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
        _eventBus.Subscribe<GameEvents.OnItemCollected>(OnItemCollected);
    }
    public void SetGoalUI(ItemsEnum item, int itemCount)
    {
        this.itemEnum = item;
        this.itemCount = itemCount;

        SetIconImage(ItemDataManager.GetSpriteFromEnum(item));
        SetCountText();

        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _eventBus?.Unsubscribe<GameEvents.OnItemCollected>(OnItemCollected);
    }

    private void OnItemCollected(GameEvents.OnItemCollected onItemCollected)
    {
        if (itemCount <= 0 || onItemCollected.item.itemEnum != itemEnum)
            return;

        itemCount--;

        if (itemCount == 0)
        {
            GoalCompleted();
        }
        
        SetCountText();
    }

    public void SetIconImage(Sprite icon)
    {
        iconImage.sprite = icon;
    }
    public void SetCountText()
    {
        numberText.text = itemCount.ToString(); ;
    }
    private void GoalCompleted()
    {
        _eventBus.Fire(new GameEvents.OnGoalCompleted());

        transform.DOScale(Vector3.one * .2f, .5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;
        });
    }
}
