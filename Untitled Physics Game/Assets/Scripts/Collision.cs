using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public ParticleSystem sparksPS;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.instance.PlayOneShot(AudioManager.AUDIO_CLIPS.CRASH);
        StartCoroutine(PlayParticles());
    }

    private IEnumerator PlayParticles()
    {
        sparksPS.Play();
        yield return new WaitForSeconds(sparksPS.main.duration);
        sparksPS.Stop();
    }
}
