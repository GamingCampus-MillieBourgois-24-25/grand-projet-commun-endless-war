using System;
using UnityEngine;

public class XPEvents
{
    public static event Action<int> OnXPPicked;
    public static event Action<int, int> OnXPChanged;
    public static event Action<int> OnLevelUP;

    public static void PickXP(int xp)
    {
        OnXPPicked?.Invoke(xp);
    }

    public static void RaiseXPChanged(int current, int max)
    {
        OnXPChanged?.Invoke(current, max);
    }

    public static void RaiseLevelChanged(int level)
    {
        OnLevelUP?.Invoke(level);
    }
}
