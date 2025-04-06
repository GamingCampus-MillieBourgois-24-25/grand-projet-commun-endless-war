using System;
using UnityEngine;

public class HealthEvents
{
    public static event Action<int, int> OnHealthChanged;

    public static void RaiseHealthChanged(int current, int max)
    {
        OnHealthChanged?.Invoke(current, max);
    }
}
