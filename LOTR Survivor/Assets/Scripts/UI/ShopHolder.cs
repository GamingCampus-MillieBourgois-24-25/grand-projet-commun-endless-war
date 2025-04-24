using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopHolder : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;
    [SerializeField] private ShopHolderSettings settings;

    [SerializeField] private Button button;

    [SerializeField] private Image bannerImage;
    [SerializeField] private TMP_Text bannerText;
    [SerializeField] private TMP_Text bannerType;
    [SerializeField] private TMP_Text cost;

    private void Awake()
    {
        button.onClick.AddListener(SelectItem);    
    }

    private void Start()
    {
        image.sprite = settings.slotSprite;
        text.text = settings.slotName;
    }

    private void SelectItem()
    {
        bannerImage.sprite = settings.slotSprite;
        bannerImage.preserveAspect = true;
        bannerText.text = settings.slotName;
        bannerType.text = settings.slotType.ToString();
        cost.text = settings.slotCost.ToString();
    }

    private void HideItem()
    {
        bannerImage.sprite = null;
        bannerImage.preserveAspect = false;
        bannerText.text = null;
        bannerType.text = null;
        cost.text = null;
    }
}
