using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    protected AttackSettings attackSettings;
    protected AudioSource audioSource;

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
            OneShotAudio.PlayClip(attackSettings.spawnClip, transform.position);
        }
    }

    protected abstract void UpdateAttack();

    protected virtual void ResetAttack() { }

    protected void DestroyAttack()
    {
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
        }
    }
}
