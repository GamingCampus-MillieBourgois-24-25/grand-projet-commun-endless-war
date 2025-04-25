using System;
using UnityEngine;

public static class PVMaxEvents
{
    public static event Action<float> OnHPObjectPicked;

    public static void PickHPObject(float percentage)
    {
        OnHPObjectPicked?.Invoke(percentage);
    }
}
