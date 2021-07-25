﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType { 
        Key,
        Sunglasses,
        CassetteTape,
        SvahiliNote
    }
    public ItemType itemType;
    public string description;
    public Texture inventoryImage;


    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Key: return ItemAssets.Instance.keySprite;
            case ItemType.Sunglasses: return ItemAssets.Instance.sunglassesSprite;
            case ItemType.CassetteTape: return ItemAssets.Instance.CassetteTapeSprite;
            case ItemType.SvahiliNote: return ItemAssets.Instance.SvahiliNoteSprite;
        }
    }

    public Color GetColor()
    {
        switch (itemType)
        {
            default:
            case ItemType.Key: return new Color(1, 1, 1);
            case ItemType.Sunglasses: return new Color(1, 0, 0);
            case ItemType.CassetteTape: return new Color(0, 0, 1);
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Key:
            case ItemType.Sunglasses:
            case ItemType.CassetteTape:
                return false;
        }
    }
}
