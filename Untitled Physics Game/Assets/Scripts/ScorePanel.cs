using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] Color enabledColor;
    [SerializeField] Color disabledColor;

    [SerializeField] Image[] stars;

    public int score = 0;

    public void AddScore()
    {
        if (stars[score] == null)
            return;

        EnableStar(stars[score]);
        score++;
    }

    public void RemoveScore()
    {
        if (stars[score] == null)
            return;

        DisableStar(stars[score]);
        score--;
    }

    private void EnableStar(Image starToEnable)
    {
        starToEnable.color = enabledColor;
    }

    private void DisableStar(Image starToEnable)
    {
        starToEnable.color = disabledColor;
    }
}
