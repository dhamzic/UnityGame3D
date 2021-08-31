using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;

    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();

        //AddItem(new Item { itemType = Item.ItemType.SilverKey, actionKey = "Key" });
        //AddItem(new Item { itemType = Item.ItemType.CassetteTape });
        //AddItem(new Item { itemType = Item.ItemType.Sunglasses });
        //AddItem(new Item { itemType = Item.ItemType.SilverKey, actionKey = "Key" });
        //AddItem(new Item { itemType = Item.ItemType.CassetteTape });
        //AddItem(new Item { itemType = Item.ItemType.Sunglasses });
    }
    public void AddItem(Item item)
    {
        itemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public void RemoveItem(Item item)
    {
        itemList.Remove(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
}
