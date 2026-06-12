using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DunkController : MonoBehaviour
{
    public static event Action<bool> OnRimContactChanged;

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
    private int _rimContactCount;
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

        var velocity = _rb.velocity;
        if (velocity.x != 0)
        {
            velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * 10f);
            _rb.velocity = velocity;
        }
    }

    private void Jump()
    {
        // Vector2 velocity = _rb.velocity;

        // velocity.y = 0f;

        // _rb.velocity = velocity;
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

    //private bool _touched;

    //private Tween _delay;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsHoopCollision(collision))
        {
            _touchedRim = true;
            SetRimContact(true);
            //_touched = true;

            //_delay?.Kill();
            //_delay = DOVirtual.DelayedCall(0.5f, () => _touched = false);

            // var velocity = _rb.velocity;
            // velocity.x = 0f;
            // _rb.velocity = velocity;
            AudioManager.Ins.PlayScore();
            return;
        }

        if (IsBottomBoundary(collision.transform))
        {
            GameManager.Ins.GameOver("Dunk touched the bottom boundary.");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsHoopCollision(collision)) return;

        _rimContactCount = Mathf.Max(0, _rimContactCount - 1);

        if (_rimContactCount == 0)
        {
            OnRimContactChanged?.Invoke(false);
        }
    }

    public void ResetRimState()
    {
        _touchedRim = false;
    }

    private void OnDisable()
    {
        ClearRimContact();
    }

    private void OnDestroy()
    {
        _wingFlapSequence?.Kill();
        ClearRimContact();
    }

    private void ClearRimContact()
    {
        if (_rimContactCount > 0)
        {
            _rimContactCount = 0;
            OnRimContactChanged?.Invoke(false);
        }
    }

    private void SetRimContact(bool isTouching)
    {
        if (!isTouching) return;

        _rimContactCount++;

        if (_rimContactCount == 1)
        {
            OnRimContactChanged?.Invoke(true);
        }
    }

    private bool IsHoopCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hoop"))
        {
            return true;
        }

        return collision.transform.GetComponentInParent<IHoop>() != null;
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
