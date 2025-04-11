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

            // Applique le volume SFX actuel
            instance.setVolume(VolumeManager.Instance.GetSFXVolume());

            // Positionnement 3D si nécessaire
            if (position != default)
            {
                instance.set3DAttributes(position.To3DAttributes());
            }

            instance.start();
            instance.release();
        }
    }
}