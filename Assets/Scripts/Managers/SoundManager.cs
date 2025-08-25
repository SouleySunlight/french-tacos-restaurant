using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambientSource;

    public static bool isMusicOn { get; private set; } = true;
    public static bool areSoundsOn { get; private set; } = true;
    [SerializeField] private AudioClip mainTheme;

    public void PlayMusic()
    {
        if (!isMusicOn)
        {
            musicSource.Stop();
            return;
        }
        musicSource.clip = mainTheme;
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        musicSource.Play();
    }

    public void TurnOnMusic()
    {
        isMusicOn = true;
        PlayMusic();
    }

    public void TurnOffMusic()
    {
        isMusicOn = false;
        PlayMusic();

    }

    public void TurnOnSounds()
    {
        areSoundsOn = true;
        ambientSource.volume = 0.5f;

    }

    public void TurnOffSounds()
    {
        areSoundsOn = false;
        ambientSource.volume = 0f;

    }

    public void PlaySFX(AudioClip clip)
    {
        if (!areSoundsOn)
        {
            return;
        }
        sfxSource.PlayOneShot(clip);
    }

    public void PlayAmbient(AudioClip clip)
    {
        if (!areSoundsOn)
        {
            ambientSource.volume = 0f;
        }
        else
        {
            ambientSource.volume = 0.5f;
        }
        ambientSource.clip = clip;
        ambientSource.loop = true;
        ambientSource.Play();
    }
    public void StopAmbient()
    {
        ambientSource.Stop();
    }

    public void LoadIsMusicOn(bool musicOn)
    {
        isMusicOn = musicOn;
        if (isMusicOn)
        {
            PlayMusic();
        }
    }

    public void LoadAreSoundsOn(bool soundsOn)
    {
        areSoundsOn = soundsOn;
    }

}