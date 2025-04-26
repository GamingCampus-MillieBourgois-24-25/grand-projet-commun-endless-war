using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float scaleDuration = 1.2f;
    [SerializeField] private float targetScale = 1.1f;
    [SerializeField] private float waitBeforeLoad = 0.5f;

    private void Start()
    {
        PlaySplashScreenAnimation();
    }

    private void PlaySplashScreenAnimation()
    {
        rectTransform.localScale = Vector3.one * 0.8f;
        canvasGroup.alpha = 0f;

        canvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.InOutQuad);
        rectTransform.DOScale(targetScale, scaleDuration).SetEase(Ease.OutQuad)
            .OnComplete(() => StartCoroutine(LoadNextScene()));
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(waitBeforeLoad);
        Loader.Load(Loader.Scene.MenuScene);
    }
}
