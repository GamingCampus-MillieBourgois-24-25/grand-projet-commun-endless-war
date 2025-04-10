using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPMagnetEvents : MonoBehaviour
{
    public static Action<Vector3, float> OnMagnetTriggered;

    public static void Trigger(Vector3 position, float radius)
    {
        OnMagnetTriggered?.Invoke(position, radius);
    }
}