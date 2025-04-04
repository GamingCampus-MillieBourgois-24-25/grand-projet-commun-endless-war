using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    protected int damage;
    protected float speed;
    protected GameObject attackPrefab;

    public virtual void Initialize(int damage, float speed, GameObject prefab)
    {
        this.damage = damage;
        this.speed = speed;
        this.attackPrefab = prefab;

        ResetAttack();
    }

    protected abstract void UpdateAttack();

    protected virtual void ResetAttack() { }

    protected void DestroyAttack()
    {
        PlayFX();
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.Despawn(gameObject, attackPrefab);
        }
        else
        {
            Debug.Log("ObjectPool Instance is not present in the scene!");
            Destroy(gameObject);
        }
    }

    protected virtual void PlayFX() { }
}
