using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "newShopSlot", menuName = "Shop/ShopSlot")]
public class ShopHolderSettings : ScriptableObject
{
    public Sprite slotSprite;
    public string slotName;
    public string slotCost;
    public string slotType;
    public CostType costType;
    public int size = 0;

    [TextArea(3, 10)]
    public string maskText = "";
}

public enum SlotType
{
    Icon,
    Item,
    LegendarySkin,
    CommonSkin
}

public enum CostType
{
    Money,
    Mythril,
    Gold
}
