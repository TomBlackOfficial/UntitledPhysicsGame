using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpriteOnHover : MonoBehaviour
{
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        sprite.color = Random.ColorHSV(0, 1, 1, 1, 1, 1);
    }
}
