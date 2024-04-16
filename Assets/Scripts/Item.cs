using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : IItem
{
    public Rigidbody rb;
    public Collider collider;

    public bool isMatched;

    public override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);

        transform.localScale = Vector3.one;
        isMatched = false;

        collider.enabled = true;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    public override void ItemClicked()
    {
        base.ItemClicked();

        collider.enabled = false;
        rb.useGravity = false;
        rb.isKinematic = true;

        _eventBus.Fire(new GameEvents.OnItemCollected(this));   
    }


}
