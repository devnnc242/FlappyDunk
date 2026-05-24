using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DunkController : MonoBehaviour
{
    [Header("Fly")]
    [SerializeField] private float jumpForce = 8f;

    [Header("Forward")]
    [SerializeField] private float forwardSpeed = 3f;

    [Header("Input")]
    [SerializeField] private bool readInputDirectly = true;

    private Rigidbody2D _rb;
    private bool _touchedRim;

    public bool TouchedRim => _touchedRim;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (readInputDirectly && IsTapDown())
        {
            JumpForward();
        }
    }

    public void Fly()
    {
        JumpForward();
    }

    private void JumpForward()
    {
        Vector2 velocity = _rb.velocity;

        velocity.x = forwardSpeed;
        velocity.y = 0f;

        _rb.velocity = velocity;
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
        }
    }

    public void ResetRimState()
    {
        _touchedRim = false;
    }
}
