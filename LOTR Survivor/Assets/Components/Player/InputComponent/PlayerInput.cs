using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Vector2 JoystickSize = new Vector2(100, 100);
    [SerializeField]
    private FloatingJoyStick Joystick;
    [SerializeField]
    private Rigidbody PlayerRb;
    [SerializeField] 
    private float speed = 5f;
    [SerializeField] 
    private float rotationSpeed = 10f;

    private Finger MovementFinger;
    private Vector2 MovementAmount;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnEnable()
    {
        TouchSimulation.Enable();
        Touch.onFingerDown += HandleFingerDown;
        Touch.onFingerUp += HandleFingerUp;
        Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        Touch.onFingerDown -= HandleFingerDown;
        Touch.onFingerUp -= HandleFingerUp;
        Touch.onFingerMove -= HandleFingerMove;
        TouchSimulation.Disable();
    }

    private void HandleFingerMove(Finger MovedFinger)
    {
        if (MovedFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = JoystickSize.x / 2f;
            Touch currentTouch = MovedFinger.currentTouch;

            if (Vector2.Distance(currentTouch.screenPosition, Joystick.RectTransform.anchoredPosition) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition;
            }

            Joystick.Knob.anchoredPosition = knobPosition;
            MovementAmount = knobPosition / maxMovement;
        }
    }

    private void HandleFingerUp(Finger LostFinger)
    {
        if (LostFinger == MovementFinger)
        {
            MovementFinger = null;
            Joystick.Knob.anchoredPosition = Vector2.zero;
            Joystick.gameObject.SetActive(false);
            MovementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        if (MovementFinger == null)
        {
            MovementFinger = TouchedFinger;
            MovementAmount = Vector2.zero;
            Joystick.gameObject.SetActive(true);
            Joystick.RectTransform.sizeDelta = JoystickSize;
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 StartPosition)
    {
        if (StartPosition.x < JoystickSize.x / 2)
        {
            StartPosition.x = JoystickSize.x / 2;
        }
        else if (StartPosition.x > Screen.width - JoystickSize.x / 2)
        {
            StartPosition.x = Screen.width - JoystickSize.x / 2;
        }

        if (StartPosition.y < JoystickSize.y / 2)
        {
            StartPosition.y = JoystickSize.y / 2;
        }
        else if (StartPosition.y > Screen.height - JoystickSize.y / 2)
        {
            StartPosition.y = Screen.height - JoystickSize.y / 2;
        }

        return StartPosition;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(MovementAmount.x, 0, MovementAmount.y) * speed;

        PlayerRb.velocity = new Vector3(movement.x, PlayerRb.velocity.y, movement.z);

        if (movement != Vector3.zero)
        {
            //Pour une rotation réaliste
            //Quaternion targetRotation = Quaternion.LookRotation(movement);
            //PlayerRb.rotation = Quaternion.Slerp(PlayerRb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            //Pour une rotation instantanée
            PlayerRb.rotation = Quaternion.LookRotation(movement);
        }
    }

    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle()
        {
            fontSize = 24,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };
        if (MovementFinger != null)
        {
            GUI.Label(new Rect(10, 35, 500, 20), $"Finger Start Position: {MovementFinger.currentTouch.startScreenPosition}", labelStyle);
            GUI.Label(new Rect(10, 65, 500, 20), $"Finger Current Position: {MovementFinger.currentTouch.screenPosition}", labelStyle);
        }
        else
        {
            GUI.Label(new Rect(10, 35, 500, 20), "No Current Movement Touch", labelStyle);
        }

        GUI.Label(new Rect(10, 10, 500, 20), $"Screen Size ({Screen.width}, {Screen.height})", labelStyle);
    }
}
