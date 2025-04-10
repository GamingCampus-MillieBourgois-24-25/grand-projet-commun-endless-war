using System;
using UnityEngine;

public class HealthEvents
{
    public static event Action<int, int> OnHealthChanged;
    public static event Action<Transform> OnRevive;
    public static event Action<Transform> OnReviveComplete;
    public static event Action OnPlayerDeath;
    public static event Action<int> OnPlayerDamaged;

    public static event Action OnGameOver;


    public static void RaiseHealthChanged(int current, int max)
    {
        OnHealthChanged?.Invoke(current, max);
    }

    public static void Revive(Transform player)
    {
        OnRevive?.Invoke(player);
    }

    public static void ReviveFinished(Transform player)
    {
        OnReviveComplete?.Invoke(player);
    }

    public static void PlayerDeathEvent()
    {
        OnPlayerDeath?.Invoke();
    }
    public static void PlayerDamagedEvent(int amount)
    {
        OnPlayerDamaged?.Invoke(amount);
    }

    public static void GameOver()
    {
        OnGameOver?.Invoke();
    }
}
