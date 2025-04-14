using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBarCanvas : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider healthDepleteSlider;
    [SerializeField] private float transitionDuration = 1f;

    private void OnEnable()
    {
        HealthEvents.OnHealthChanged += UpdateUI;
    }

    private void OnDisable()
    {
        HealthEvents.OnHealthChanged -= UpdateUI;
    }

    public void UpdateUI(int current, int max)
    {
        healthSlider.maxValue = max;
        healthDepleteSlider.maxValue = max;

        float health = current;

        healthSlider.DOKill();
        healthDepleteSlider.DOKill();

        if (health < healthSlider.value)
        {
            healthSlider.value = health;

            healthDepleteSlider.DOKill();
            healthDepleteSlider.DOValue(health, transitionDuration)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true);
        }
        else
        {
            healthSlider.DOValue(health, transitionDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true);

            healthDepleteSlider.DOValue(health, transitionDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true);
        }
    }
}
