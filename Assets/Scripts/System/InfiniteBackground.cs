using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform bg1;
    [SerializeField] private Transform bg2;

    [Header("Settings")]
    [SerializeField] private float scrollSpeed = 2f;

    private float _bgWidth;

    private void Start()
    {
        SpriteRenderer sr = bg1.GetComponent<SpriteRenderer>();
        _bgWidth = sr.sprite.bounds.size.x;

        bg2.position = new Vector3(bg1.position.x + _bgWidth, bg1.position.y, bg1.position.z);
    }

    private void Update()
    {
        float delta = scrollSpeed * Time.deltaTime;

        bg1.position += Vector3.left * delta;
        bg2.position += Vector3.left * delta;

        CheckAndReset(bg1, bg2);
        CheckAndReset(bg2, bg1);
    }

    private void CheckAndReset(Transform target, Transform other)
    {
        if (target.position.x + _bgWidth / 2f < Camera.main.transform.position.x - GetCameraHalfWidth())
        {
            float newX = other.position.x + _bgWidth;
            target.position = new Vector3(newX, target.position.y, target.position.z);
        }
    }

    private float GetCameraHalfWidth()
    {
        Camera cam = Camera.main;
        return cam.orthographicSize * cam.aspect;
    }

    public void SetSpeed(float speed)
    {
        scrollSpeed = speed;
    }
}
