﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    //public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    //{
    //    Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);
    //    //Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);
    //    //GameObject ob = Instantiate(OpenCloseTextParent ,position, Quaternion.identity);


    //    ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
    //    itemWorld.SetItem(item);

    //    return itemWorld;
    //}
    public Item.ItemType itemType;
    public string itemDescription;
    private SpriteRenderer spriteRenderer;

    //public void SetItem(Item item)
    //{
    //    this.item = item;
    //    spriteRenderer.sprite = item.GetSprite();
    //}
    //public Item GetItem()
    //{
    //    return item;
    //}
}
