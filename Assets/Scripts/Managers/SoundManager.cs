using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambientSource;

    public static bool isMusicOn { get; private set; } = true;


    [SerializeField] private AudioClip mainTheme;

    void Start()
    {
        PlayMusic();
    }

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

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayAmbient(AudioClip clip)
    {
        ambientSource.clip = clip;
        ambientSource.loop = true;
        ambientSource.volume = 0.5f;
        ambientSource.Play();
    }
    public void StopAmbient()
    {
        ambientSource.Stop();
    }



}