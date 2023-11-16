using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text header, footer;
    public Image[] cars;
    public Color p1Color, p2Color;

    public Sprite[] p1Sprites, p2Sprites;

    private void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
    }

    private void Awake()
    {
        p1Sprites = Resources.LoadAll<Sprite>("GameOver/p1");
        p2Sprites = Resources.LoadAll<Sprite>("GameOver/p2");
    }

    private void OnGameOver(bool p1Won)
    {
        if(p1Won)
        {
            for(int i = 0; i < 5; i++)
            {
                cars[i].sprite = p1Sprites[i];
            }
            footer.text = "congratulations player 1!";
            header.color = p1Color;
            footer.color = p1Color;
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                cars[i].sprite = p2Sprites[i];
            }
            footer.text = "congratulations player 2!";
            header.color = p2Color;
            footer.color = p2Color;
        }
    }
}
