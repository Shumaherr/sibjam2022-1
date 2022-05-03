using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Box : MonoBehaviour
{
    public Sprite stickerSprite;

    private List<Color> colors = new List<Color>()
    {
        Color.red,
        Color.white,
        Color.green,
        Color.cyan,
        Color.magenta
    };

    public readonly Color color;

    void Start()
    {
        var sticker = new GameObject
        {
            transform =
            {
                parent = transform,
                localPosition = new Vector3(0, 0, -1),
                localScale = new Vector3(4f, 4f, 1f)
            }
        };

        var spriteSticker = sticker.AddComponent<SpriteRenderer>();
        spriteSticker.sprite = stickerSprite;

        var color = colors[Random.Range(0, colors.Count)];
        spriteSticker.color = color;
    }

    private void Update()
    {
      
    }
}