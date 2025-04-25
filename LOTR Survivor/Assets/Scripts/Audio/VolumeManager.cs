using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    private static VolumeManager _instance;
    public static VolumeManager Instance => _instance;

    [SerializeField] private float musicVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;

    [SerializeField] private AudioSource soundFXPrefab;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        Debug.Log($"[FMOD] Volume musique demandé : {musicVolume}");

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.UpdateMusicVolume(musicVolume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;

    public void PlaySFX(AudioClip audioClip, float volume = 1f, Transform spawnTransform = null, bool persistThroughScenes = false)
    {
        if (audioClip == null || soundFXPrefab == null) return;

        Vector3 spawnPosition = spawnTransform != null ? spawnTransform.position : Vector3.zero;

        AudioSource audioSource = Instantiate(soundFXPrefab, spawnPosition, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = Mathf.Clamp01(volume) * sfxVolume;
        audioSource.Play();

        if (persistThroughScenes)
            DontDestroyOnLoad(audioSource.gameObject);

        Destroy(audioSource.gameObject, audioClip.length);
    }

}