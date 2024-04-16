using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public struct ItemsStruct
{
    public ItemsEnum item;
    public Sprite itemSprite;
    public GameObject itemObject;
}
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    public List<ItemsStruct> items = new List<ItemsStruct>();
}