using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip mainTheme;

    void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        musicSource.clip = mainTheme;
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        musicSource.Play();
    }



}