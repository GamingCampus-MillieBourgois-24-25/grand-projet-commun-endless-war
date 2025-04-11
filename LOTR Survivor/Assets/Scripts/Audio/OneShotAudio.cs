using UnityEngine;

public class OneShotAudio : MonoBehaviour
{
    public static void PlayClip(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        GameObject tempGO = new GameObject("OneShotAudio");
        tempGO.transform.position = position;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.spatialBlend = 1f;
        aSource.Play();

        float clipLength = clip.length;
        Object.Destroy(tempGO, clipLength);
    }
}
