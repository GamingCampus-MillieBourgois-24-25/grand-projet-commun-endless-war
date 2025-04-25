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
    [SerializeField] private Button shopButton;

    [SerializeField] private TMP_Text musicVolumeText;
    [SerializeField] private TMP_Text sfxVolumeText;

    [SerializeField] public GameObject mainMenu;
    [SerializeField] private GameObject optionMenu;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private string nextScene;

    [SerializeField] private MusicManager musicManager;

    [SerializeField] private ShopCanvas shopCanvas;

    private void Start()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);


        playButton.onClick.AddListener(ChangeScene);
        optionButton.onClick.AddListener(OptionMenu);
        shopButton.onClick.AddListener(ShopMenu);

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

    void ShopMenu()
    {
        mainMenu.SetActive(false);
        shopCanvas.OpenShop();
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
