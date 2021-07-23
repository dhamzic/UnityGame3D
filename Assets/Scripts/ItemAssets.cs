using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton klasa
/// </summary>
public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Debug.Log("ItemAssets Awake");
    }


    public Transform pfItemWorld;

    public Sprite keySprite;
    public GameObject keyObject;

    public Sprite sunglassesSprite;
    public Sprite CassetteTapeSprite;
    public Sprite SvahiliNoteSprite;
}
