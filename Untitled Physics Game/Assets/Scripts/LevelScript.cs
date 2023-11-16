using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public Transform p1SpawnPoint, p2SpawnPoint;

    [SerializeField] LevelEffect[] levelEffects;
    [SerializeField] int timeToActivateEffects;

    private void Start()
    {
        Invoke(nameof(ActivateLevelEffects), timeToActivateEffects);
    }

    private void ActivateLevelEffects()
    {
        foreach (LevelEffect effect in levelEffects)
        {
            effect.ActivateEffect();
        }
    }
}
