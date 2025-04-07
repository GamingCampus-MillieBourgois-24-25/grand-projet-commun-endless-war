using System;
using UnityEngine;

public class XPEvents
{
    public static event Action<int> OnXPPicked;
    public static event Action<int, int> OnXPChanged;

    public static void PickXP(int xp)
    {
        OnXPPicked?.Invoke(xp);
    }

    public static void RaiseXPChanged(int current, int max)
    {
        OnXPChanged?.Invoke(current, max);
    }
}
