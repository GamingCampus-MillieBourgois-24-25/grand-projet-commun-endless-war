using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/Projectile Settings", fileName = "NewProjectileSettings")]
public class ProjectileSettings : ScriptableObject
{
    [Tooltip("Damage")]
    public int Damage = 10;

    [Tooltip("Speed")]
    public float Speed = 10f;

    [Tooltip("Cooldown")]
    public float Cooldown = 1f;

    [Tooltip("Prefab")]
    public GameObject prefab;

    [Header("Explosion")]
    [Tooltip("Prefab explosion")]
    public GameObject explosionPrefab;

    [Tooltip("Sound explosion")]
    public AudioClip explosionSound;
}
