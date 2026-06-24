using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DunkController : MonoBehaviour
{
    public static event Action<bool> OnRimContactChanged;

    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private DunkWingAnimator wingAnimator;

    [Header("Movement")]
    [SerializeField] private float jumpForce = 8f;

    [Header("Game over")]
    [SerializeField] private string bottomBoundaryName = "Boundary_2";

    private Rigidbody2D _rb;
    private GameManager _gameManager;

    private bool _touchedRim;
    private int _rimContactCount;
    private int _lastFlyFrame = -1;

    public bool TouchedRim => _touchedRim;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _gameManager = GameManager.Ins;
    }

    private void OnEnable()
    {
        if (playerInput != null) playerInput.OnTap += Fly;
    }

    private void OnDisable()
    {
        if (playerInput != null) playerInput.OnTap -= Fly;

        ClearRimContact();
    }

    private void Update()
    {
        Vector2 veclocity = _rb.velocity;

        if (Mathf.Abs(veclocity.x) > 0.01f)
        {
            veclocity.x = Mathf.Lerp(veclocity.x, 0f, Time.deltaTime * 10f);

            _rb.velocity = veclocity;
        }
    }

    public void Fly()
    {
        if (_lastFlyFrame == Time.frameCount) return;

        _lastFlyFrame = Time.frameCount;

        if (_gameManager.IsGameOver) return;

        if (_gameManager.IsReady) _gameManager.StartGame();

        if (!_gameManager.IsPlaying) return;

        Jump();
    }

    private void Jump()
    {
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        AudioManager.Ins.PlaySound(SoundType.Jump);

        wingAnimator?.PlayFlap();
    }

    public void ResetRimState()
    {
        _touchedRim = false;
    }

    private void SetRimContact(bool touching)
    {
        if (!touching) return;

        _rimContactCount++;

        if (_rimContactCount == 1) OnRimContactChanged?.Invoke(true);
    }

    private void ClearRimContact()
    {
        if (_rimContactCount <= 0) return;

        _rimContactCount = 0;

        OnRimContactChanged?.Invoke(false);
    }

    private bool IsHoopCollision(Collision2D collision)
    {
        return collision.transform.GetComponentInParent<HoopBase>() != null;
    }

    private bool IsBottomBoundary(Transform t)
    {
        if (t.CompareTag("GameOver")) return true;

        if (t.name == bottomBoundaryName) return true;

        return t.parent != null && t.parent.name == "LowerLimit";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsHoopCollision(collision))
        {
            _touchedRim = true;

            SetRimContact(true);

            return;
        }

        if (IsBottomBoundary(collision.transform))
        {
            _gameManager.GameOver("Dunk touched bottom boundary");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsHoopCollision(collision)) return;

        _rimContactCount = Mathf.Max(0, _rimContactCount - 1);

        if (_rimContactCount == 0) OnRimContactChanged?.Invoke(false);
    }
}
