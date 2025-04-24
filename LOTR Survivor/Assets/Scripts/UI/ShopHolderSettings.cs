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
    public int slotCost;
    public SlotType slotType;
    public CostType costType;
}

public enum SlotType
{
    Icon,
    Item
}

public enum CostType
{
    Money,
    Mythril,
    Gold
}
