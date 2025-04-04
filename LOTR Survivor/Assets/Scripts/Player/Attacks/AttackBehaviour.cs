using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehaviour : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField] protected AttackSettings attackSettings;
    [SerializeField] protected LayerMask enemyLayer;

    protected float attackTimer;

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
    public void SetProjectileSettings(AttackSettings newProjectileSettings)
    {
        attackSettings = newProjectileSettings;
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
}
