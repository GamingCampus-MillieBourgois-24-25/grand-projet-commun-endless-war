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
    public void SetAttackSettings(AttackSettings newAttackSettings)
    {
        attackSettings = newAttackSettings;
    }

    protected bool CanAttack()
    {
        return attackTimer >= attackSettings.Cooldown;
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

    public virtual void Upgrade()
    {
        Debug.Log($"{name} has been upgraded! New damage: {attackSettings.Damage}");
    }
}
