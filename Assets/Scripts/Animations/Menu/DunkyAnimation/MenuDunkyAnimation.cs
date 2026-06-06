using DG.Tweening;
using UnityEngine;

public class MenuDunkyAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform leftWing;
    [SerializeField] private RectTransform rightWing;

    private void Start()
    {
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DORotate(
            new Vector3(0, 0, -15f),
            0.3f));

        seq.Join(
            leftWing.DORotate(
                new Vector3(0, 0, -25f),
                0.3f));

        seq.Join(
            rightWing.DORotate(
                new Vector3(0, 0, -25f),
                0.3f));

        seq.Append(transform.DORotate(
            new Vector3(0, 0, 15f),
            0.3f));

        seq.Join(
            leftWing.DORotate(
                new Vector3(0, 0, 25f),
                0.3f));

        seq.Join(
            rightWing.DORotate(
                new Vector3(0, 0, -25f),
                0.3f));

        seq.Append(transform.DORotate(
            Vector3.zero,
            0.4f));

        seq.Join(
            transform.DOScale(
                1.077f,
                0.4f));

        seq.Append(
            transform.DOScale(
                1f,
                0.2f));

        seq.SetLoops(-1);
    }
}
