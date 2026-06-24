using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Dunk")) return;

        GameManager.Ins.GameOver("Hit Spike!");
    }
}
