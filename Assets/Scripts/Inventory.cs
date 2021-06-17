using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();

        //AddItem(new Item { itemType = Item.ItemType.Key, description = "Key" });
        //AddItem(new Item { itemType = Item.ItemType.CassetteTape, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.Sunglasses, amount = 1 });
    }
    public void AddItem(Item item)
    {
        itemList.Add(item);
        Debug.Log(item.itemType.ToString() + " has been collected in the Inventory. Current list count: "+this.itemList.Count+"");
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

}
