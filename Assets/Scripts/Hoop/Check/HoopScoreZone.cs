using UnityEngine;

public class HoopScoreZone : MonoBehaviour
{
    private bool _hasScored;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasScored) return;

        if (!collision.CompareTag("Dunk")) return;

        Rigidbody2D rb = collision.attachedRigidbody;

        if (rb.velocity.y > 0f) return;

        DunkController dunk = collision.GetComponent<DunkController>();

        if (dunk == null) return;

        ScoreManager.Ins.ProcessScore(true);

        dunk.ResetRimState();

        _hasScored = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Dunk")) return;

        _hasScored = false;
    }
}