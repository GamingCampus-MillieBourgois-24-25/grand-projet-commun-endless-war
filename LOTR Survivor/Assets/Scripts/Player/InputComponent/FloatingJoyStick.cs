using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingJoyStick : MonoBehaviour
{
    public RectTransform RectTransform;
    public RectTransform Knob;
    public StickType stickType;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }
}

public enum StickType
{
    Move,
    Turn
}
