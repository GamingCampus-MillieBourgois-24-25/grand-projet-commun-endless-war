using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEvent : MonoBehaviour
{
    public static BombEvent Instance;

    public event Action OnKillAllEnemies;
    public event Action OnKillAllVisibleEnemies;

    public void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void KillAllEnemies()
    {
        OnKillAllEnemies?.Invoke();
    }

    /*public void KillAllVisibleEnemies()
    {
        OnKillAllVisibleEnemies?.Invoke();
    }*/
}
