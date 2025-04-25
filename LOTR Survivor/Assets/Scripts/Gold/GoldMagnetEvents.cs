using System;
using UnityEngine;

public static class GoldMagnetEvents
{
    public static event Action<Vector3, float> OnMagnetTriggered;

    public static void Trigger(Vector3 position, float radius)
    {
        OnMagnetTriggered?.Invoke(position, radius);
    }
}
