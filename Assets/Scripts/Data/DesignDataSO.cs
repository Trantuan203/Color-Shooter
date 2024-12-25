using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DesignData", menuName = "Design/DesignData")]
public class DesignDataSO : ScriptableObject
{
    public List<ItemVisual> itemVisuals = new();

    public ItemVisual GetItemVisual(CharacterColor characterColor)
    {
        return itemVisuals.Find(item => item.characterColor == characterColor);
    }
}

[Serializable]
public class ItemVisual
{
    public CharacterColor characterColor;
    public Sprite img;
}