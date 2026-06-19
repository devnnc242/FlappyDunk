using UnityEngine;

public class MovingHoop : HoopBase
{
    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private float amplitude = 1f;

    private Vector3 _startPos;

    public override HoopType Type => HoopType.Moving;

    private void OnEnable()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        transform.position = _startPos + Vector3.up * Mathf.Sin(Time.time * moveSpeed) * amplitude;
    }

    public override void ResetState()
    {
        base.ResetState();

        _startPos = transform.position;
    }
}
