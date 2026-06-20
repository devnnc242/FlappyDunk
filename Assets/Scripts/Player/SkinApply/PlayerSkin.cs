// using UnityEngine;

// public class PlayerSkin : MonoBehaviour
// {
//     [SerializeField] private SpriteRenderer ballRenderer;
//     [SerializeField] private TrailRenderer trail;

//     private void Start()
//     {
//         ApplySkin();
//     }

//     public void ApplySkin()
//     {
//         SkinData skin = SkinManager.Ins.currentSkin;

//         ballRenderer.sprite = skin.ballSprite;

//         trail.material = skin.trailMaterial;

//         trail.startColor = skin.trailColor;
//         trail.endColor = new Color(skin.trailColor.r, skin.trailColor.g, skin.trailColor.b, 0f);
//     }
// }
