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
    }

    public Sprite keySilverSprite;
    public Sprite keyGoldenSprite;
    public Sprite sunglassesSprite;
    public Sprite CassetteTapeSprite;
    public Sprite SvahiliNoteSprite;
    public Sprite BookSprite;
}
