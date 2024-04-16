using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using static GameEvents;

public class TileParent : MonoBehaviour
{
    private EventBus _eventBus;


    public List<Item> items = new List<Item>();
    public List<Transform> tiles;

    private readonly float moveObjectTime = .2f;
    private readonly float matchTime = .25f;


    public void Initialize()
    {
        _eventBus = ServiceLocator.Instance.Resolve<EventBus>();

        _eventBus.Subscribe<GameEvents.OnItemCollected>(OnItemCollected);
        _eventBus.Subscribe<GameEvents.OnLevelLoaded>(OnLevelLoaded);

    }
    private void OnLevelLoaded()
    {
        items.Clear();
    }
    private void OnItemCollected(GameEvents.OnItemCollected onItemCollected)
    {
        AddItemToMergeTile(onItemCollected.item);
    }

    private void AddItemToMergeTile(Item item)
    {
        if (items.Count == 0) 
        {
            items.Add(item);
            MoveItemToBasket(0, item);
        }
        else
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].itemEnum == item.itemEnum && !items[i].isMatched)
                {
                    items.Insert(i + 1, item);
                    MoveItemToBasket(i + 1, item);
                    MoveObjectOnTile();
                    return;
                }
            }

            items.Add(item);
            MoveItemToBasket(items.Count - 1, item);
        }
    }
    private void MoveItemToBasket(int i, Item item) // Clicked Item Going to Tile
    {
        if (i < 7)
        {
            var targetPos = GetPositionOfTile(i) + Vector3.up * .5f+ Vector3.forward*.2f ;

            item.transform.DOJump(targetPos,2F,1 , moveObjectTime).SetEase(Ease.Linear);
            item.transform.DORotate(Vector3.zero,  moveObjectTime, RotateMode.Fast).SetEase(Ease.Linear);
            item.transform.DOScale(Vector3.one*.7f, moveObjectTime).SetEase(Ease.Linear);
        }
        Run.After(moveObjectTime, CheckMatch);
    }
    public void MoveObjectOnTile() // Item movement on tile
    {
        for (int i = 0; i < items.Count; i++)
        {
            var targetPos = GetPositionOfTile(i) + Vector3.up * .5f + Vector3.forward * .2f;

            if (items[i] && Vector3.Distance(items[i].transform.position,targetPos)>.1f)
                items[i].transform.DOMove(targetPos, moveObjectTime).SetEase(Ease.Linear);
        }
    }
    private Vector3 GetPositionOfTile(int i)
    {
        return tiles[i].position;
    }
    private void CheckBasketIsFull()
    {
        if (items.Count == 7)
        {
            _eventBus.Fire(new GameEvents.OnLevelFailed());
            return;
        }
    }
    private void CheckMatch()
    {
        for (int i = 0; i < items.Count-2; i++)
        {
            if (!items[i].isMatched &&items[i].itemEnum == items[i+1].itemEnum &&
                items[i].itemEnum == items[i+2].itemEnum)
            {
                MatchMove(items[i], items[i+1], items[i+2]);
                return;
            }
        }
        CheckBasketIsFull();
    }
    private void MatchMove(Item leftItem,Item middleItem,Item rightItem)
    {
        DestroyItem(leftItem,middleItem.transform.position);
        DestroyItem(rightItem, middleItem.transform.position);
        DestroyItem(middleItem, middleItem.transform.position);

        Run.After(matchTime, () =>
        {
            MoveObjectOnTile();
        });
    }
    private void DestroyItem(Item item,Vector3 middleItemPos)
    {
        item.isMatched = true;
        item.transform.DOMove(middleItemPos, matchTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            items.Remove(item);
            Destroy(item.gameObject);
        });      
    }

}
