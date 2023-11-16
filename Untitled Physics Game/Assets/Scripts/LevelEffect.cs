using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEffect : MonoBehaviour
{
    protected bool activated;

    public virtual void ActivateEffect()
    {
        activated = true;
    }
}
