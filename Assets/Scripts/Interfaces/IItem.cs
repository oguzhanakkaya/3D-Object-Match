using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameEvents;

public abstract class IItem : MonoBehaviour
{
    public EventBus _eventBus;

    public ItemsEnum itemEnum;

    public virtual void Awake()
    {
        _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
    }
    public virtual void ItemClicked() { }
}
