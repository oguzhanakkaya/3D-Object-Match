using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDataManager
{
    public static ItemData itemData=Resources.Load<ItemData>("ItemsData");

    public static GameObject GetObjectFromEnum(ItemsEnum en)
    {
        return itemData.items.Find(x => x.item == en).itemObject;
    }
    public static Sprite GetSpriteFromEnum(ItemsEnum en)
    {
        return itemData.items.Find(x => x.item == en).itemSprite;
    }
}
