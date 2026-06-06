using UnityEngine;
using UnityEngine.UI;

public class SkinItemUI : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject selected;

    private SkinData _skin;

    public void Setup(SkinData data)
    {
        _skin = data;

        icon.sprite = _skin.icon;

        Refresh();
    }

    public void OnClick()
    {
        if (!_skin.defaultUnclocked) return;

        EquipmentManager.Ins.Equip(_skin);

        FindObjectOfType<SkinMenuUI>().RefreshAll();
    }

    public void Refresh()
    {
        bool unlocked = _skin.defaultUnclocked;

        background.color = unlocked ? Color.white : Color.gray;

        string equipped = EquipmentManager.Ins.GetEquippedId(_skin.type);

        selected.SetActive(equipped == _skin.id);
    }
}
