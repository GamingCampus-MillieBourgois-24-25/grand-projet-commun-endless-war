using UnityEngine;
using FMODUnity;

public abstract class Attack : MonoBehaviour
{
    protected AttackSettings attackSettings;
    protected AudioSource audioSource;
    protected GameObject player;

    protected virtual void Awake()
    {
        TryPlaySpawnSound();
    }

    public void SetSettings(AttackSettings newProjectileSettings)
    {
        if (newProjectileSettings == null)
        {
            Debug.LogWarning("New AttackSettings is null.");
            return;
        }

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
            if (attackSettings.prefab != null)
            {
                ObjectPool.Instance.Despawn(gameObject, attackSettings.prefab);
            }
            else
            {
                Debug.LogWarning("Prefab is null, cannot despawn.");
            }
        }
        else
        {
            Debug.LogError("ObjectPool Instance is not present in the scene!");
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

    protected void ApplyAttackEffects(GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("Target is null. Skipping attack effects.");
            return;
        }

        foreach (var effect in attackSettings.attackEffects)
        {
            StatusEffectUtils.Apply(effect, target, player);
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
