using UnityEngine;

public class OneShotAudio : MonoBehaviour
{
    public static void PlayClip(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null)
            return;

        GameObject audioObject = new GameObject("OneShotAudio");
        audioObject.transform.position = position;

        AudioSource source = audioObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = Mathf.Clamp01(volume);
        source.spatialBlend = 1f;
        source.Play();

        Object.Destroy(audioObject, clip.length);
    }
}
