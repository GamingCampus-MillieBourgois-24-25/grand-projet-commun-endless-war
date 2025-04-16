using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehaviour : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField] protected AttackSettings attackSettings;
    [SerializeField] protected LayerMask enemyLayer;

    protected float attackTimer;
    protected int skillLevel = 1;
    protected GameObject player;

    private void Start()
    {

    }

    protected virtual void Update()
    {
        attackTimer += Time.deltaTime;

        if (CanAttack())
        {
            Attack();
            attackTimer = 0f;
        }
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }

    public void SetAttackSettings(AttackSettings newAttackSettings)
    {
        attackSettings = newAttackSettings;
        attackSettings.Reset();
    }

    protected virtual bool CanAttack()
    {
        float actualCooldown = attackSettings.Cooldown;

        actualCooldown *= PlayerStatsMultiplier.cooldownMultiplier;

        actualCooldown = Mathf.Max(actualCooldown, attackSettings.MinCooldown);

        return attackTimer >= actualCooldown;
    }


    protected GameObject SpawnOrInstantiate(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (ObjectPool.Instance != null)
        {
            return ObjectPool.Instance.Spawn(prefab, position, rotation);
        }
        else
        {
            return Instantiate(prefab, position, rotation);
        }
    }

    protected abstract void Attack();

    public virtual void Upgrade(AttackSettings newAttackSettings = null)
    {
        if (newAttackSettings != null)
        {
            SetAttackSettings(newAttackSettings);
            skillLevel = 1;
        }
        else
        {
            skillLevel++;
            attackSettings = attackSettings.Upgrade(skillLevel);
            Debug.Log("New level : " + skillLevel);
        }
    }

    protected void ApplyAttackEffects()
    {
        foreach (var effect in attackSettings.attackEffects)
        {
            StatusEffectUtils.Apply(effect, null, player);
        }
    }

    protected void ApplyStatusEffects(GameObject target)
    {
        foreach (var effect in attackSettings.statusEffects)
        {
            StatusEffectUtils.Apply(effect, target, player);
        }
    }

    protected void ApplyBuffs()
    {
        foreach (var effect in attackSettings.buffEffects)
        {
            PlayerStatsMultiplier.ApplyBuff(effect);
        }
    }
}
