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

        //AddItem(new Item { itemType = Item.ItemType.SilverKey, description = "Key" });
        //AddItem(new Item { itemType = Item.ItemType.CassetteTape });
        //AddItem(new Item { itemType = Item.ItemType.Sunglasses });
        //AddItem(new Item { itemType = Item.ItemType.SilverKey, description = "Key" });
        //AddItem(new Item { itemType = Item.ItemType.CassetteTape });
        //AddItem(new Item { itemType = Item.ItemType.Sunglasses });
    }
    public void AddItem(Item item)
    {
        itemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log(item.itemType.ToString() + " has been collected in the Inventory. Current list count: " + this.itemList.Count + "");
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in this.itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null)
            {
                itemList.Remove(itemInInventory);
            }
        }
        else
        {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
}
