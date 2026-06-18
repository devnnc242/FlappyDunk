using DG.Tweening;
using UnityEngine;

public class HoopScoredAnimation : MonoBehaviour
{
    [SerializeField] private float scoredScale = 1.25f;
    [SerializeField] private float scoredDuration = 0.35f;
    [SerializeField] private Ease scoredEase = Ease.OutBack;
    [SerializeField] private float disappearDelay = 0.5f;

    private Vector3 _defaultScale;

    private SpriteRenderer[] _renderers;

    private Collider2D[] _colliders;

    private Color[] _defaultColor;

    private Sequence _sequence;

    private void Aake()
    {
        _defaultScale = transform.localScale;

        _renderers = GetComponentsInChildren<SpriteRenderer>(true);

        _colliders = GetComponentsInChildren<Collider2D>(true);

        _defaultColor = new Color[_renderers.Length];

        for (int i = 0; i < _renderers.Length; i++)
        {
            _defaultColor[i] = _renderers[i].color;
        }
    }

    public void ResetState()
    {
        _sequence?.Kill();

        transform.localScale = _defaultScale;

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].color = _defaultColor[i];
        }

        foreach (Collider2D col in _colliders)
        {
            col.enabled = true;
        }
    }

    public void Play()
    {
        DOVirtual.DelayedCall(disappearDelay, PlayInternal).SetLink(gameObject);
    }

    private void PlayInternal()
    {
        foreach (Collider2D col in _colliders)
        {
            col.enabled = false;
        }

        _sequence?.Kill();

        _sequence = DOTween.Sequence();

        _sequence.Join(transform.DOScale(_defaultScale * scoredScale, scoredDuration));

        foreach (SpriteRenderer sr in _renderers)
        {
            _sequence.Join(sr.DOFade(0f, scoredDuration));
        }

        _sequence.OnComplete(() => gameObject.SetActive(false));
    }
}
