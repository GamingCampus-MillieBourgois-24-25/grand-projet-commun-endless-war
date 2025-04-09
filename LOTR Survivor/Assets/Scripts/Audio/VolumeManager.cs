using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    private static VolumeManager _instance;
    public static VolumeManager Instance => _instance;

    [HideInInspector] public float musicVolume = 1f;
    [HideInInspector] public float sfxVolume = 1f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);  // Keep this object between scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        // Optionally update the music source here if needed
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }

    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;
}
