using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Music")]
    [SerializeField] private AudioSource musicSource;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    //[SerializeField] private AudioClip buttonClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip scoreClip;
    //[SerializeField] private AudioClip gameOverClip;

    // public void PlayButton()
    // {
    //     PlaySfx(buttonClip);
    // }

    public void PlayJump()
    {
        PlaySfx(jumpClip);
    }

    public void PlayScore()
    {
        PlaySfx(scoreClip);
    }

    // public void PlayGameOver()
    // {
    //     PlaySfx(gameOverClip);
    // }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;

        if (musicSource.clip == clip)
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    private void PlaySfx(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;

        sfxSource.PlayOneShot(clip);
    }
}
