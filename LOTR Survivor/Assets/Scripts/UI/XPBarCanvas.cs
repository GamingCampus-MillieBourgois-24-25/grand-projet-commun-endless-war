using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.PlayerLoop;

public class XPBarCanvas : MonoBehaviour
{
    [SerializeField] private Slider xpSlider;
    [SerializeField] private float transitionDuration = 1f;

    private Tween xpTween;
    private void OnEnable()
    {
        XPEvents.OnXPChanged += UpdateXP;
    }

    private void OnDisable()
    {
        XPEvents.OnXPChanged -= UpdateXP;
    }

    public void UpdateXP(int currentXP, int maxXP)
    {
        if (xpTween != null && xpTween.IsActive())
        {
            xpTween.Kill();
        }

        xpSlider.maxValue = maxXP;

        xpSlider
            .DOValue(currentXP, transitionDuration)
            .SetEase(Ease.OutCubic);
    }
}

