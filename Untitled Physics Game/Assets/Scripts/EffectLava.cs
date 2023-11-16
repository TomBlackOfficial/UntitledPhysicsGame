using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLava : LevelEffect
{
    [SerializeField] float moveSpeed = 0.5f;

    public override void ActivateEffect()
    {
        base.ActivateEffect();
    }

    private void Update()
    {
        if (!activated)
            return;

        transform.position += (Vector3)(Vector2.up * moveSpeed * Time.deltaTime);
    }
}
