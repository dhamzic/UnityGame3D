using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType { 
        SilverKey,
        GoldenKey,
        Sunglasses,
        CassetteTape,
        SvahiliNote,
        Book
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
            case ItemType.SilverKey: return ItemAssets.Instance.keySilverSprite;
            case ItemType.GoldenKey: return ItemAssets.Instance.keyGoldenSprite;
            case ItemType.Sunglasses: return ItemAssets.Instance.sunglassesSprite;
            case ItemType.CassetteTape: return ItemAssets.Instance.CassetteTapeSprite;
            case ItemType.SvahiliNote: return ItemAssets.Instance.SvahiliNoteSprite;
            case ItemType.Book: return ItemAssets.Instance.BookSprite;
        }
    }

    //Je li objekt za skupljanje ili je točno 1
    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.GoldenKey:
            case ItemType.Sunglasses:
            case ItemType.CassetteTape:
                return false;
        }
    }
}
