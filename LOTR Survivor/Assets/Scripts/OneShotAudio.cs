using UnityEngine;

public class OneShotAudio
{
    public static void PlayClip(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null)
            return;

        AudioSource.PlayClipAtPoint(clip, position, Mathf.Clamp01(volume));
    }
}