using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameEvents;

public class InputManager : MonoBehaviour
{
    private EventBus _eventBus;

    public bool canClick;

    public void Initialize()
    {
        _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
        _eventBus.Subscribe<GameEvents.OnLevelLoaded>(OnLevelLoaded);
        _eventBus.Subscribe<GameEvents.OnLevelEnded>(OnLevelEnded);
    }

    private void OnLevelLoaded() => canClick = true;  
    private void OnLevelEnded()  => canClick = false;

    void Update()
    {
        if (canClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                    if (hit.collider != null)
                        if (hit.collider.gameObject.TryGetComponent<IItem>(out IItem item) && item != null)
                        {
                            item.ItemClicked();
                        }

            }
        }
       
    }
}
