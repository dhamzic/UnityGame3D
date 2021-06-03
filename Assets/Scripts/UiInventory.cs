using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour
{

    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    //private Player player;

    void Start()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
    }

    //public void SetPlayer(Player player)
    //{
    //    this.player = player;
    //}

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        //inventory.OnItemListChanged += Inventory_OnItemListChanged;

        if (itemSlotTemplate == null || itemSlotContainer == null)
        {
            itemSlotContainer = transform.Find("itemSlotContainer");
            itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
            RefreshInventoryItems();
        }
        else {
            RefreshInventoryItems();
        }
    }

    private void RefreshInventoryItems()
    {
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 75f;
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            x++;
            if (x >= 4)
            {
                x = 0;
                y--;
            }
        }
    }

}
