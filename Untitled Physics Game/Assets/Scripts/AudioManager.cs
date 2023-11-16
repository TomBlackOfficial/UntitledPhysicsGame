using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonTemplate<AudioManager>
{
    public AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }
}
