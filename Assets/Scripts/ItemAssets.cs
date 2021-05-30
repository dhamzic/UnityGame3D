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


    public Transform prefabItemWorld;

    public Sprite keySprite;
    public Sprite sunglassesSprite;
    public Sprite CassetteTapeSprite;
}
