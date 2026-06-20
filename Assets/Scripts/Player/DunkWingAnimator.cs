using DG.Tweening;
using UnityEngine;

public class DunkWingAnimator : MonoBehaviour
{
    [SerializeField] private Transform leftWing;
    [SerializeField] private Transform rightWing;

    [SerializeField] private Vector3 leftWingDownRotation = new(0, 0, -35);
    [SerializeField] private Vector3 rightWingDownRotation = new(0, 0, 35);

    [SerializeField] private float wingDownDuration = 0.08f;
    [SerializeField] private float wingUpDuration = 0.12f;

    private Vector3 _leftDefault;
    private Vector3 _rightDefault;

    private Sequence _sequence;

    private void Aake()
    {
        if (leftWing != null) _leftDefault = leftWing.localEulerAngles;

        if (rightWing != null) _rightDefault = rightWing.localEulerAngles;
    }

    public void PlayFlap()
    {
        if (leftWing == null && rightWing == null) return;

        _sequence?.Kill();

        _sequence = DOTween.Sequence();

        if (leftWing != null)
        {
            leftWing.localEulerAngles = _leftDefault;

            _sequence.Join(leftWing.DOLocalRotate(_leftDefault + leftWingDownRotation,
            wingDownDuration).SetEase(Ease.OutQuad));
        }

        if (rightWing != null)
        {
            rightWing.localEulerAngles = _rightDefault;

            _sequence.Join(rightWing.DOLocalRotate(_rightDefault + rightWingDownRotation,
            wingDownDuration).SetEase(Ease.OutQuad));
        }

        if (leftWing != null)
        {
            _sequence.Append(leftWing.DOLocalRotate(_leftDefault, wingUpDuration).SetEase(Ease.OutBack));
        }

        if (rightWing != null)
        {
            Tween rightUp = rightWing.DOLocalRotate(_rightDefault, wingUpDuration).SetEase(Ease.OutBack);

            if (leftWing != null) _sequence.Join(rightUp);
            else _sequence.Append(rightUp);
        }
    }

    private void OnDestroy()
    {
        _sequence?.Kill();
    }
}
