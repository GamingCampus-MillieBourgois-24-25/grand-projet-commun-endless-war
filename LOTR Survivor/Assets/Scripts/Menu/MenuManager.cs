using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button quitMenu;

    [SerializeField] private TMP_Text playText;
    [SerializeField] private TMP_Text musicVolumeText;
    [SerializeField] private TMP_Text sfxVolumeText;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionMenu;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private string nextScene;

    [SerializeField] private MusicManager musicManager;

    private void Start()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);

        StartCoroutine(FadeTextRoutine());

        playButton.onClick.AddListener(ChangeScene);
        quitMenu.onClick.AddListener(ReturnMenu);
        optionButton.onClick.AddListener(OptionMenu);

        musicSlider.onValueChanged.AddListener(delegate { AdjustMusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { AdjustSFXVolume(); });

        if (musicSource != null)
        {
            musicSlider.value = VolumeManager.Instance.GetMusicVolume();
            musicSource.Play();
        }

        sfxSlider.value = VolumeManager.Instance.GetSFXVolume();
        UpdateVolumeTexts();
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
        Loader.Load(Loader.Scene.HubScene);
    }

    void OptionMenu()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    void ReturnMenu()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
    }

    void AdjustMusicVolume()
    {
        VolumeManager.Instance.SetMusicVolume(musicSlider.value);

        if (musicManager != null)
        {
            musicManager.UpdateMusicVolume(musicSlider.value);
        }

        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        UpdateVolumeTexts();
    }

    void AdjustSFXVolume()
    {
        VolumeManager.Instance.SetSFXVolume(sfxSlider.value);

        AudioSource[] allSFXSources = FindObjectsOfType<AudioSource>();
        foreach (var sfxSource in allSFXSources)
        {
            if (sfxSource != musicSource)
                sfxSource.volume = sfxSlider.value;
        }

        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        UpdateVolumeTexts();
    }

    void UpdateVolumeTexts()
    {
        if (musicVolumeText != null)
            musicVolumeText.text = Mathf.RoundToInt(musicSlider.value * 100) + "%";

        if (sfxVolumeText != null)
            sfxVolumeText.text = Mathf.RoundToInt(sfxSlider.value * 100) + "%";
    }
}
