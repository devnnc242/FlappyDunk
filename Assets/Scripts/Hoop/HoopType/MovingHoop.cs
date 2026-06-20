using UnityEngine;

public class MovingHoop : HoopBase
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private Transform visual;

    private Vector3 _defaultLocalPos;
    private float _moveTimer;

    public override HoopType Type => HoopType.Moving;

    private MovingMode _moveMode;
    private GameManager _gameManager;

    protected override void Awake()
    {
        base.Awake();

        _defaultLocalPos = visual.localPosition;

        _gameManager = GameManager.Ins;
    }

    public override void ResetState()
    {
        base.ResetState();

        _moveTimer = 0f;

        visual.localPosition = _defaultLocalPos;

        _moveMode = Random.value > 0.5f ? MovingMode.Vertical : MovingMode.Horizontal;
    }

    public override void TickMovement()
    {
        _moveTimer += Time.deltaTime;

        float offset = Mathf.Sin(_moveTimer * moveSpeed) * amplitude;

        Vector3 pos = _defaultLocalPos;

        switch (_moveMode)
        {
            case MovingMode.Vertical:
                pos.y += offset;
                break;

            case MovingMode.Horizontal:
                pos.x += offset;
                break;
        }

        visual.localPosition = pos;
    }
}
