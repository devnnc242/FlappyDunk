using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offsetX = 3f;
    [SerializeField] private float smoothTime = 0.2f;

    private float _fixedY;
    private float _fixedZ;
    private float _veclocityX;

    private void Start()
    {
        _fixedY = transform.position.y;
        _fixedZ = transform.position.z;
    }

    private void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        float targetX = target.position.x + offsetX;

        float smoothX = Mathf.SmoothDamp(transform.position.x, targetX, ref _veclocityX, smoothTime);

        transform.position = new Vector3(smoothX, _fixedY, _fixedZ);
    }
}
