using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBarCanvas : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float transitionDuration = 1f;

    public void UpdateUI(float health, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;

        healthSlider.DOValue(health, transitionDuration).SetEase(Ease.OutCubic);
    }
}
