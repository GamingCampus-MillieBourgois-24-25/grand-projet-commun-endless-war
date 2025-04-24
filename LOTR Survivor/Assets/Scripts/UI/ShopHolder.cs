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
    [SerializeField] private Image bannerImageBig;

    [SerializeField] private Image bannerMask;
    [SerializeField] private Image bannerMaskBig;

    [SerializeField] private TMP_Text bannerText;
    [SerializeField] private TMP_Text bannerType;
    [SerializeField] private TMP_Text cost;

    [SerializeField] private TMP_Text maskText;

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
        if (settings.size == 0)
        {
            bannerImage.sprite = settings.slotSprite;
            bannerImage.preserveAspect = true;

            var imageColor = bannerImage.color;
            imageColor.a = 1f;
            bannerImage.color = imageColor;

            var maskColor = bannerMask.color;
            maskColor.a = 1f;
            bannerMask.color = maskColor;

            var maskBigColor = bannerMaskBig.color;
            maskBigColor.a = 0f;
            bannerMaskBig.color = maskBigColor;
        }
        else
        {
            bannerImageBig.sprite = settings.slotSprite;
            bannerImageBig.preserveAspect = true;

            var imageColor = bannerImageBig.color;
            imageColor.a = 1f;
            bannerImageBig.color = imageColor;

            var maskBigColor = bannerMaskBig.color;
            maskBigColor.a = 1f;
            bannerMaskBig.color = maskBigColor;

            var maskColor = bannerMask.color;
            maskColor.a = 0f;
            bannerMask.color = maskColor;
        }

        maskText.text = settings.maskText;
        bannerText.text = settings.slotName;
        bannerType.text = settings.slotType;
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
