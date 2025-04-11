using UnityEngine;
using FMODUnity;

public abstract class Attack : MonoBehaviour
{
    protected AttackSettings attackSettings;

    protected virtual void Awake()
    {
        TryPlaySpawnSound();
    }

    public void SetSettings(AttackSettings newProjectileSettings)
    {
        attackSettings = newProjectileSettings;
        TryPlaySpawnSound();
    }

    private void TryPlaySpawnSound()
    {
        if (attackSettings != null && attackSettings.spawnEvent.IsNull == false)
        {
            OneShotAudio.Play(attackSettings.spawnEvent, transform.position);
        }
    }

    private void TryPlayHitSound()
    {
        if (attackSettings != null && attackSettings.hitEvent.IsNull == false)
        {
            OneShotAudio.Play(attackSettings.hitEvent, transform.position);
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
        }
    }
}