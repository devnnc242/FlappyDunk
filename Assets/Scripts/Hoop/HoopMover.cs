using System;
using DG.Tweening;
using UnityEngine;

public class HoopMover : MonoBehaviour, IHoop
{
    public static event Action<HoopMover> OnHoopPassed;
    public static event Action<HoopMover> OnHoopMissed;

    //IHoop
    public GameObject Hoop => gameObject;
    public bool IsScored => _scored;

    [Header("Scored Animation")]
    [SerializeField] private float scoredScale = 1.25f;
    [SerializeField] private float scoredDuration = 0.35f;
    [SerializeField] private Ease scoredEase = Ease.OutBack;
    [SerializeField] private float disappearDelay = 0.5f;

    private bool _scored;
    private Vector3 _defaultScale;
    private SpriteRenderer[] _spriteRenderers;
    private Color[] _defaultColors;
    private Collider2D[] _colliders;
    private Sequence _scoredSequence;

    private void Awake()
    {
        _defaultScale = transform.localScale;
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        _colliders = GetComponentsInChildren<Collider2D>(true);

        _defaultColors = new Color[_spriteRenderers.Length];

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _defaultColors[i] = _spriteRenderers[i].color;
        }
    }

    public void ResetState()
    {
        _scoredSequence?.Kill();
        _scoredSequence = null;

        _scored = false;
        transform.localScale = _defaultScale;

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _spriteRenderers[i].color = _defaultColors[i];
        }

        foreach (Collider2D col in _colliders)
        {
            col.enabled = true;
        }
    }

    public void MarkScored()
    {
        if (_scored) return;

        _scored = true;

        OnHoopPassed?.Invoke(this);

        //PlayScoredAnimation();
        DOVirtual.DelayedCall(disappearDelay, PlayScoredAnimation);
    }

    public void OnReadchedLimit()
    {
        if (_scored)
        {
            gameObject.SetActive(false);
            return;
        }

        OnHoopMissed?.Invoke(this);
        gameObject.SetActive(false);
    }

    private void PlayScoredAnimation()
    {
        foreach (Collider2D col in _colliders)
        {
            col.enabled = false;
        }

        _scoredSequence?.Kill();
        _scoredSequence = DOTween.Sequence();

        _scoredSequence
            .Join(transform.DOScale(_defaultScale * scoredScale, scoredDuration).SetEase(scoredEase));

        foreach (SpriteRenderer sr in _spriteRenderers)
        {
            _scoredSequence.Join(sr.DOFade(0f, scoredDuration).SetEase(Ease.OutQuad));
        }

        _scoredSequence.OnComplete(() => gameObject.SetActive(false));
    }
}
