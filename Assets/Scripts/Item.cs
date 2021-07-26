using System.Collections;
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
    public int id;


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

    //Je li objekt za skupljanje ili je točno 1
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
