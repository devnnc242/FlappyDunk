using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : Singleton<EquipmentManager>
{
    public void Equip(SkinData skin)
    {
        PlayerPrefs.SetString(skin.type.ToString(), skin.id);

        PlayerPrefs.Save();
    }

    public string GetEquippedId(SkinType type)
    {
        return PlayerPrefs.GetString(type.ToString(), "");
    }
}
