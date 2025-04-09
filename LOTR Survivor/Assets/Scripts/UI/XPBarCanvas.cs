using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.PlayerLoop;

public class XPBarCanvas : MonoBehaviour
{
    [SerializeField] private Slider xpSlider;
    [SerializeField] private float transitionDuration = 0.5f;

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
        xpSlider.maxValue = maxXP;
        if (currentXP == 0)
        {
            xpSlider.value = 0;
            return;
        }

        if (xpTween != null && xpTween.IsActive())
        {
            xpTween.Kill();
        }

        xpSlider
            .DOValue(currentXP, transitionDuration)
            .SetEase(Ease.Linear)
            .SetUpdate(true);
    }
}

