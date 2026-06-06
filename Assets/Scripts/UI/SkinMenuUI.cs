using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinMenuUI : MonoBehaviour
{
    [SerializeField] private SkinData[] skins;
    [SerializeField] private SkinItemUI prefab;
    [SerializeField] private Transform content;

    private SkinType _currentType = SkinType.Ball;

    private readonly List<SkinItemUI> _items = new();

    [Header("Tab")]
    [SerializeField] private Image ballIcon;
    [SerializeField] private Image wingIcon;
    //[SerializeField] private Image hoopIcon;
    [SerializeField] private Image hatIcon;

    [SerializeField] private Color selectedColor = Color.blue;
    [SerializeField] private Color normalColor = Color.black;

    private void Start()
    {
        ShowTab(SkinType.Ball);
    }

    private void ShowTab(SkinType type)
    {
        _currentType = type;

        UpdateTabVisual(type);

        Generate();
    }

    public void ShowBallTab()
    {
        ShowTab(SkinType.Ball);
    }

    public void ShowWingTab()
    {
        ShowTab(SkinType.Wing);
    }

    public void ShowHatTab()
    {
        ShowTab(SkinType.Hat);
    }

    private void UpdateTabVisual(SkinType type)
    {
        ballIcon.color =
            type == SkinType.Ball
            ? selectedColor
            : normalColor;

        wingIcon.color =
            type == SkinType.Wing
            ? selectedColor
            : normalColor;

        // hoopIcon.color =
        //     type == SkinType.Hoop
        //     ? selectedColor
        //     : normalColor;

        hatIcon.color =
            type == SkinType.Hat
            ? selectedColor
            : normalColor;
    }

    private void Generate()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        _items.Clear();

        foreach (SkinData skin in skins)
        {
            if (skin.type != _currentType) continue;

            SkinItemUI item = Instantiate(prefab, content);

            item.Setup(skin);

            _items.Add(item);
        }
    }

    public void RefreshAll()
    {
        foreach (var item in _items)
        {
            item.Refresh();
        }
    }
}
