using FMODUnity;
using FMOD.Studio;
using UnityEngine;

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

        // V�rification suppl�mentaire avant de cr�er l'instance
        if (!musicEvent.IsNull)
        {
            try
            {
                musicInstance = RuntimeManager.CreateInstance(musicEvent);
                musicInstance.start();
                UpdateMusicVolume(VolumeManager.Instance.GetMusicVolume());
            }
            catch (EventNotFoundException e)
            {
                Debug.LogError($"�v�nement FMOD non trouv�: {e.Message}");
                Debug.LogError($"V�rifiez que l'�v�nement '{musicEvent.Path}' existe dans vos banques FMOD");
            }
        }
        else
        {
            Debug.LogError("La r�f�rence � l'�v�nement musical n'est pas assign�e dans l'inspecteur");
        }
    }

    public void UpdateMusicVolume(float volume)
    {
        if (musicInstance.isValid())
        {
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