using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonTemplate<AudioManager>
{
    public AudioSource audioSource;

    public AudioClip mainMenu, playing, engine, crash1, crash2, options, jump, honk;

    private bool clipDonePlaying;

    public enum GAME_STATES
    {
        MAINMENU,
        PLAYING
    }
    public GAME_STATES STATE;
    
    public enum AUDIO_CLIPS
    {
        HONK,
        CRASH,
        OPTION,
        ENGINE,
        JUMP
    }
    private GAME_STATES CLIP;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        STATE = GAME_STATES.MAINMENU;
        UpdateBGM(STATE);
    }

    public void UpdateBGM(GAME_STATES state)
    {
        audioSource.loop = true;
        STATE = state;

        switch(state)
        {
            case(GAME_STATES.MAINMENU):
                audioSource.clip = mainMenu;
                break;
            case(GAME_STATES.PLAYING):
                audioSource.clip = playing;
                break;
            default:
                audioSource.clip = mainMenu;
                break;
        }

        audioSource.Play();
    }

    public void PlayOneShot(AUDIO_CLIPS clip)
    {
        switch(clip)
        {
            case(AUDIO_CLIPS.HONK):
                audioSource.PlayOneShot(honk);
                break;
            case(AUDIO_CLIPS.JUMP):
                audioSource.PlayOneShot(jump);
                break;
            case(AUDIO_CLIPS.CRASH):
                audioSource.PlayOneShot(Random.value < 0.5f ? crash1 : crash2);
                break;
            case(AUDIO_CLIPS.OPTION):
                audioSource.PlayOneShot(options);
                break;
        }
    }

    private IEnumerator PlaySoundTillDone(AudioClip clip)
    {
        yield return null; 

        if(!clipDonePlaying)
        {
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
        clipDonePlaying = true;
    }
}
