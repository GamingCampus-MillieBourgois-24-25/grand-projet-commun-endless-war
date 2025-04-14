using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private EventReference musicEvent;
    private EventInstance musicInstance;

    private static MusicManager _instance;
    public static MusicManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        LoadFMODBanks();
        PlayMusic();
    }

    private void LoadFMODBanks()
    {
        try
        {
            RuntimeManager.LoadBank("Master", true);

            Debug.Log("[FMOD] Banques chargées avec succès.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[FMOD] Erreur lors du chargement des banques : {e.Message}");
        }
    }

    private void PlayMusic()
    {
        if (musicEvent.IsNull)
        {
            Debug.LogError("[FMOD] L'événement musical n'est pas assigné dans l'inspecteur !");
            return;
        }

        musicInstance = RuntimeManager.CreateInstance(musicEvent);

        if (!musicInstance.isValid())
        {
            Debug.LogError("[FMOD] Impossible de créer l'instance de musique !");
            return;
        }

        musicInstance.setVolume(1.0f);
        musicInstance.start();

        Debug.Log("[FMOD] Musique lancée au démarrage");
    }

    public void UpdateMusicVolume(float volume)
    {
        if (musicInstance.isValid())
        {
            Debug.Log($"[FMOD] Volume musique appliqué : {volume}");
            musicInstance.setVolume(volume);
        }
    }

    private void OnDestroy()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
        }
    }
}