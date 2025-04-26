using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class SoundButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float scaleFactor = 1.1f;
    [SerializeField] private float duration = 0.1f;
    [SerializeField] private Transform buttonTransform;

    private Vector3 initialScale;

    private void Start()
    {
        if (buttonTransform == null)
        {
            buttonTransform = transform;
        }
        initialScale = buttonTransform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound)
            VolumeManager.Instance.PlaySFX(clickSound, 1f, transform, true);

        buttonTransform.DOScale(initialScale * scaleFactor, duration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                buttonTransform.DOScale(initialScale, duration)
                .SetUpdate(true);
            });
    }
}
