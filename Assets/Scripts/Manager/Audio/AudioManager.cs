using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Database")]
    [SerializeField] private AudioData[] audioDatabase;

    private readonly Dictionary<SoundType, AudioClip> _clips = new();

    private GameManager _gameManager;

    protected override void Awake()
    {
        base.Awake();

        foreach (AudioData data in audioDatabase)
        {
            if (!_clips.ContainsKey(data.type))
            {
                _clips.Add(data.type, data.clip);
            }
        }
    }

    private void Start()
    {
        _gameManager = GameManager.Ins;

        if (_gameManager != null)
        {
            _gameManager.OnStateChanged += HandleStateChanged;
        }
    }

    private void OnDestroy()
    {
        if (_gameManager != null)
        {
            _gameManager.OnStateChanged -= HandleStateChanged;
        }
    }

    private void HandleStateChanged(GameState state)
    {
        if (state == GameState.GameOver)
        {
            PlaySound(SoundType.GameOver);
        }
    }

    public void PlaySound(SoundType type)
    {
        if (!_clips.TryGetValue(type, out AudioClip clip)) return;

        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.Play();
    }
}
