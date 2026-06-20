using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    private void OllisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Dunk")) return;

        GameManager.Ins.GameOver("Hit Spike!");
    }
}
