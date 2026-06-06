using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DunkController : MonoBehaviour
{
    [Header("Fly")]
    [SerializeField] private float jumpForce = 8f;

    [Header("Input")]
    [SerializeField] private bool readInputDirectly = true;

    [Header("Wing Animation")]
    [SerializeField] private Transform leftWing;
    [SerializeField] private Transform rightWing;
    [SerializeField] private Vector3 leftWingDownRotation = new Vector3(0f, 0f, -35f);
    [SerializeField] private Vector3 rightWingDownRotation = new Vector3(0f, 0f, 35f);
    [SerializeField] private float wingDownDuration = 0.08f;
    [SerializeField] private float wingUpDuration = 0.12f;

    [Header("Game Over")]
    [SerializeField] private string bottomBoundaryName = "Boundary_2";

    private Rigidbody2D _rb;
    private bool _touchedRim;
    private Vector3 _leftWingDefaultRotation;
    private Vector3 _rightWingDefaultRotation;
    private Sequence _wingFlapSequence;
    private int _lastFlyFrame = -1;

    public bool TouchedRim => _touchedRim;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (leftWing != null)
        {
            _leftWingDefaultRotation = leftWing.localEulerAngles;
        }

        if (rightWing != null)
        {
            _rightWingDefaultRotation = rightWing.localEulerAngles;
        }
    }

    void Update()
    {
        if (readInputDirectly && IsTapDown())
        {
            Fly();
        }
    }

    private void Jump()
    {
        Vector2 velocity = _rb.velocity;

        velocity.y = 0f;

        _rb.velocity = velocity;
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        AudioManager.Ins.PlayJump();

        PlayWingFlapAnimation();
    }

    private void PlayWingFlapAnimation()
    {
        if (leftWing == null && rightWing == null) return;

        _wingFlapSequence?.Kill();
        _wingFlapSequence = DOTween.Sequence();

        if (leftWing != null)
        {
            leftWing.localEulerAngles = _leftWingDefaultRotation;

            _wingFlapSequence.Join(
                leftWing
                    .DOLocalRotate(_leftWingDefaultRotation + leftWingDownRotation, wingDownDuration)
                    .SetEase(Ease.OutQuad)
            );
        }

        if (rightWing != null)
        {
            rightWing.localEulerAngles = _rightWingDefaultRotation;

            _wingFlapSequence.Join(
                rightWing
                    .DOLocalRotate(_rightWingDefaultRotation + rightWingDownRotation, wingDownDuration)
                    .SetEase(Ease.OutQuad)
            );
        }

        if (leftWing != null)
        {
            _wingFlapSequence.Append(
                leftWing
                    .DOLocalRotate(_leftWingDefaultRotation, wingUpDuration)
                    .SetEase(Ease.OutBack)
            );
        }

        if (rightWing != null)
        {
            Tween rightWingUpTween = rightWing
                .DOLocalRotate(_rightWingDefaultRotation, wingUpDuration)
                .SetEase(Ease.OutBack);

            if (leftWing != null)
            {
                _wingFlapSequence.Join(rightWingUpTween);
                return;
            }

            _wingFlapSequence.Append(
                rightWingUpTween
            );
        }
    }

    public void Fly()
    {
        if (_lastFlyFrame == Time.frameCount) return;
        _lastFlyFrame = Time.frameCount;

        if (GameManager.Ins.IsGameOver) return;

        if (GameManager.Ins.IsReady)
        {
            GameManager.Ins.StartGame();
        }

        if (!GameManager.Ins.IsPlaying) return;

        Jump();
    }

    private bool IsTapDown()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
#endif

        if (Input.touchCount == 0)
        {
            return false;
        }

        return Input.GetTouch(0).phase == TouchPhase.Began;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hoop"))
        {
            _touchedRim = true;
            AudioManager.Ins.PlayScore();
            return;
        }

        if (IsBottomBoundary(collision.transform))
        {
            GameManager.Ins.GameOver("Dunk touched the bottom boundary.");
        }
    }

    public void ResetRimState()
    {
        _touchedRim = false;
    }

    private void OnDestroy()
    {
        _wingFlapSequence?.Kill();
    }

    private bool IsBottomBoundary(Transform collisionTransform)
    {
        if (collisionTransform.gameObject.tag == "GameOver")
        {
            return true;
        }

        if (collisionTransform.name == bottomBoundaryName)
        {
            return true;
        }

        return collisionTransform.parent != null && collisionTransform.parent.name == "LowerLimit";
    }
}
