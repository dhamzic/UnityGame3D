using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UiInventory : MonoBehaviour
{

    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    //private Player player;

    private void Awake()
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

        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        if (itemSlotTemplate == null || itemSlotContainer == null)
        {
            //Okvir gdje se nalaze Item-i
            itemSlotContainer = transform.Find("itemSlotContainer");
            //Za svaki Item kreira se klon itemSlotTemplate-a
            itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        }
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        RefreshInventoryItems();
    }

    public void RefreshInventoryItems()
    {
        //Kod svakog dodavanja objekta crta se novi Inventory prikaz
        //tako da je potrebno obrisati stare kako ne bi došlo do duplikata
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        int y = 0;
        //Razmak između dva okvira item-a
        float itemSlotCellSize = 75f;
        foreach (Item item in inventory.GetItemList())
        {
            //Kreiranje itemSlotTemplate okvir za pojedini item.
            //itemSlotTemplate je dijete itemSlotContainera (razlog 2 argumenta)
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer)
                .GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            //Pozicija okvira pojedinog itema
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            //Dohvaća komponentu slike
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            //Postavlja sliku trenutno dohvaćenog item-a iz Inventory-a
            image.sprite = item.GetSprite();
            //Postavlja broj tipkovnice za korištenje specificnog item-a
            TextMeshProUGUI uiText = itemSlotRectTransform.Find("actionKey").GetComponent<TextMeshProUGUI>();
            uiText.SetText(item.actionKey);     
            //smjer postavljanja okvira za sljedeći item
            x++;
            if (x >= 4)
            {
                x = 0;
                y--;
            }
        }
    }
}
