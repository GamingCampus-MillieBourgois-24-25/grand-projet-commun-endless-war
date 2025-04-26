using UnityEngine;
using FMODUnity;

public abstract class Attack : MonoBehaviour
{
    protected SkillSettings skillSettings;
    protected AudioSource audioSource;
    protected GameObject player;

    protected float damageMultiplier;
    protected float projectileSpeedMultiplier;
    protected float rangeMultiplier;

    protected virtual void OnEnable()
    {
        CalculateMultipliers();
        TryPlaySpawnSound();
    }

    public void SetSettings(SkillSettings newProjectileSettings)
    {
        if (newProjectileSettings == null)
        {
            Debug.LogWarning("New SkillSettings is null.");
            return;
        }

        skillSettings = newProjectileSettings;
        TryPlaySpawnSound();
    }

    private void CalculateMultipliers()
    {
        if (PlayerStatsMultiplier.IsInitialized)
        {
            damageMultiplier = PlayerStatsMultiplier.damageMultiplier;
            projectileSpeedMultiplier = PlayerStatsMultiplier.projectileSpeedMultiplier;
            rangeMultiplier = PlayerStatsMultiplier.rangeMultiplier;
        }
        else
        {
            Debug.LogWarning("PlayerStatsMultiplier is not initialized yet, using default values.");
            damageMultiplier = 1f;
            projectileSpeedMultiplier = 1f;
            rangeMultiplier = 1f;
        }
    }

    private void TryPlaySpawnSound()
    {
        if (skillSettings != null && skillSettings.spawnEvent != null)
        {
            VolumeManager.Instance.PlaySFX(skillSettings.spawnEvent);
        }
    }

    private void TryPlayHitSound()
    {
        if (skillSettings != null && skillSettings.hitEvent != null)
        {
            VolumeManager.Instance.PlaySFX(skillSettings.hitEvent);
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
            if (skillSettings.prefab != null)
            {
                ObjectPool.Instance.Despawn(gameObject, skillSettings.prefab);
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
        if (skillSettings.hitPrefab != null)
        {
            GameObject fx = Instantiate(skillSettings.hitPrefab, transform.position, Quaternion.identity);
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

        foreach (var effect in skillSettings.attackEffects)
        {
            StatusEffectUtils.Apply(effect, target, player);
        }
    }


    protected void ApplyStatusEffects(GameObject target)
    {

        foreach (var effect in skillSettings.statusEffects)
        {
            StatusEffectUtils.Apply(effect, target, player);
        }
    }
}
