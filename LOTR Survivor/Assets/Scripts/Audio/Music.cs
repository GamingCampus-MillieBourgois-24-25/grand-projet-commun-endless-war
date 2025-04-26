using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    AudioSource musicSource;

    [SerializeField] float Volume;
    [SerializeField] float volumeMultiplier;
    [SerializeField] float pauseMultiplier = 0.2f;
    [SerializeField] float resumeMultiplier = 1f;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Volume = VolumeManager.Instance.GetMusicVolume();
        volumeMultiplier = resumeMultiplier;
    }

    private void OnEnable()
    {
        GamePauseManager.Instance.OnGamePaused += HandlePause;
        GamePauseManager.Instance.OnGameResumed += HandleResume;
    }

    private void OnDisable()
    {
        GamePauseManager.Instance.OnGamePaused -= HandlePause;
        GamePauseManager.Instance.OnGameResumed -= HandleResume;
    }

    private void Update()
    {
        musicSource.volume = Volume * volumeMultiplier;
    }

    private void HandlePause()
    {
        volumeMultiplier = pauseMultiplier;
    }

    private void HandleResume()
    {
        volumeMultiplier = resumeMultiplier;
    }
}
