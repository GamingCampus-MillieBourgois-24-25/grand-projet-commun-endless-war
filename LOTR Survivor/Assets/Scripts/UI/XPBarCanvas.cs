using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class XPBarCanvas : MonoBehaviour
{
    [SerializeField] private Slider xpSlider;
    [SerializeField] private float transitionDuration = 1f;

    public void SetMaxXp (int maxXp)
    {
        xpSlider.maxValue = maxXp;
        xpSlider.value = 0;
    }
    public void UpdateXP(int currentXP)
    {
        xpSlider.DOValue(currentXP, transitionDuration).SetEase(Ease.OutCubic);
    }
}
