using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    //Serialize//
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private TMP_Text playText;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TMP_Text volumeText;
    [SerializeField] private AudioSource musicBackground;
    private bool isVisible = false;

    void Start()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
        StartCoroutine(FadeTextRoutine());
        playButton.onClick.AddListener(ChangeScene);
        optionButton.onClick.AddListener(OptionMenu);
        musicSlider.onValueChanged.AddListener(delegate { AdjustMusicVolume(); });

        // Démarrer la musique au lancement
        musicBackground.Play();
        musicSlider.value = musicBackground.volume;
        UpdateVolumeText();
    }

    IEnumerator FadeTextRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(FadeText(0f, 1f, 0.8f));
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(FadeText(1f, 0f, 0.8f));
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = playText.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            playText.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }

    void ChangeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HubScene");
    }

    void OptionMenu()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    void AdjustMusicVolume()
    {
        musicBackground.volume = musicSlider.value;
        UpdateVolumeText();
    }

    void UpdateVolumeText()
    {
        volumeText.text = Mathf.RoundToInt(musicSlider.value * 100) + "%";
    }
}
