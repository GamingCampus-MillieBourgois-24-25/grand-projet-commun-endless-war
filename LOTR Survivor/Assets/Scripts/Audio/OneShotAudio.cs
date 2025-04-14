using FMODUnity;
using FMOD.Studio;
using UnityEngine;

public class OneShotAudio : MonoBehaviour
{
    public static void Play(EventReference soundEvent, Vector3 position = default)
    {
        if (!soundEvent.IsNull)
        {
            EventInstance instance = RuntimeManager.CreateInstance(soundEvent);

            instance.setVolume(VolumeManager.Instance.GetSFXVolume());

            instance.start();
            instance.release();
        }
    }
}