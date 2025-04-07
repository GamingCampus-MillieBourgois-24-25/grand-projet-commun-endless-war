using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class XPBarCanvas : MonoBehaviour
{
    [SerializeField] private Slider xpSlider;
    [SerializeField] private float transitionDuration = 1f;

    private Tween xpTween;

    public void SetMaxXp(int maxXp)
    {
        xpSlider.maxValue = maxXp;
        xpSlider.value = 0;
    }

    public void UpdateXP(int currentXP, Action onCompleteCallback = null)
    {
        if (xpTween != null && xpTween.IsActive())
        {
            xpTween.Kill();
        }


        xpSlider
            .DOValue(currentXP, transitionDuration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => onCompleteCallback?.Invoke());
    }
}

