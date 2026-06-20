using UnityEngine;

public class HoopScoreZone : MonoBehaviour
{
    private bool _hasScored;
    private IHoop _hoop;

    private void Awake()
    {
        _hoop = GetComponentInParent<IHoop>();
    }

    private void OnEnable() => _hasScored = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Ins.IsGameOver) return;

        if (_hasScored) return;

        if (!collision.CompareTag("Dunk")) return;

        Rigidbody2D rb = collision.attachedRigidbody;
        DunkController dunk = collision.GetComponent<DunkController>();

        if (rb == null || dunk == null) return;

        bool isFalling = rb.velocity.y <= 0f;
        bool enteredFromAbove = collision.bounds.center.y > transform.position.y;

        if (!isFalling || !enteredFromAbove)
        {
            GameManager.Ins.GameOver("Dunk went through hoop from below");
            return;
        }

        ScoreManager.Ins.ProcessScore(dunk.TouchedRim);
        AudioManager.Ins.PlayScore();
        dunk.ResetRimState();

        _hasScored = true;

        _hoop?.MarkScored();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Dunk")) return;

        _hasScored = false;
    }
}
