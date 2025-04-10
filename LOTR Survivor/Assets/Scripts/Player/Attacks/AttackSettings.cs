using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/Projectile Settings", fileName = "NewProjectileSettings")]
public class AttackSettings : ScriptableObject
{
    [Tooltip("Damage")]
    public int Damage = 10;

    [Tooltip("Speed")]
    public float Speed = 10f;

    [Tooltip("Cooldown")]
    public float Cooldown = 1f;

    [Tooltip("Range")]
    public float Range = 10f;

    [Tooltip("Scale")]
    public float Scale = 10f;

    [Tooltip("AimRange")]
    public float AimRange = 5f;

    [Tooltip("MaxRotation")]
    public float MaxRotation = 360f;

    [Tooltip("Prefab")]
    public GameObject prefab;

    [Header("Hit")]
    [Tooltip("Prefab Hit")]
    public GameObject hitPrefab;

    [Header("Audio")]
    [Tooltip("Spawn Sound")]
    public AudioClip spawnClip;

    [Tooltip("Hit Sound")]
    public AudioClip hitClip;
}
