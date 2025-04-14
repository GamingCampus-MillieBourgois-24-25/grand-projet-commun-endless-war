using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    protected AttackSettings attackSettings;
    protected AudioSource audioSource;
    protected GameObject player;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        TryPlaySpawnSound();
    }

    public void SetSettings(AttackSettings newProjectileSettings)
    {
        attackSettings = newProjectileSettings;

        TryPlaySpawnSound();
    }

    private void TryPlaySpawnSound()
    {
        if (attackSettings != null && attackSettings.spawnClip != null)
        {
            float sfxVolume = VolumeManager.Instance.GetSFXVolume();
            OneShotAudio.PlayClip(attackSettings.spawnClip, transform.position, sfxVolume);
        }
    }

    private void TryPlayHitSound()
    {
        if (attackSettings != null && attackSettings.hitClip != null)
        {
            float sfxVolume = VolumeManager.Instance.GetSFXVolume();
            OneShotAudio.PlayClip(attackSettings.hitClip, transform.position, sfxVolume);
        }
    }

    protected abstract void UpdateAttack();

    protected virtual void ResetAttack() { }

    protected void DestroyAttack()
    {

        TryPlayHitSound();
        PlayHitFX();

        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.Despawn(gameObject, attackSettings.prefab);
        }
        else
        {
            Debug.Log("ObjectPool Instance is not present in the scene!");
            Destroy(gameObject);
        }
    }

    protected virtual void PlayHitFX()
    {
        if (attackSettings.hitPrefab != null)
        {
            GameObject fx = Instantiate(attackSettings.hitPrefab, transform.position, Quaternion.identity);
            ParticleSystem system = fx.GetComponent<ParticleSystem>();
            var main = system.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            main.loop = false;


            AudioSource fxAudioSource = fx.GetComponent<AudioSource>();
            if (fxAudioSource != null)
            {
                fxAudioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            }
        }
    }

    protected void ApplyStatusEffects(GameObject target)
    {
        foreach (var effect in attackSettings.statusEffects)
        {
            StatusEffectUtils.Apply(effect, target, player);
        }
    }
}
