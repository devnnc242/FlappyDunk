using UnityEngine;

[CreateAssetMenu(menuName = "Menu/Skin")]
public class SkinData : ScriptableObject
{
    public string id;

    public SkinType type;

    public Sprite icon;

    public Sprite gameplaySprite;

    public bool defaultUnclocked;
}
