using System.Collections;
using System.Collections.Generic;
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

    private Finger movementFinger;
    private Vector2 movementAmount;

    private bool isInputEnabled = true;

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

        GamePauseManager.Instance.OnGamePaused += DisableInput;
        GamePauseManager.Instance.OnGameResumed += EnableInput;
    }

    private void OnDisable()
    {
        Touch.onFingerDown -= HandleFingerDown;
        Touch.onFingerUp -= HandleFingerUp;
        Touch.onFingerMove -= HandleFingerMove;
        TouchSimulation.Disable();

        GamePauseManager.Instance.OnGamePaused -= DisableInput;
        GamePauseManager.Instance.OnGameResumed -= EnableInput;

        ResetJoystick()
;    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (movementFinger != null || !isInputEnabled) return;

        movementFinger = touchedFinger;
        movementAmount = Vector2.zero;
        ActivateJoystick(touchedFinger.screenPosition);
    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger != movementFinger || !isInputEnabled) return;

        Vector2 knobPosition = CalculateKnobPosition(movedFinger);
        Joystick.Knob.anchoredPosition = knobPosition;
        movementAmount = knobPosition / (JoystickSize.x / 2f);
    }

    private void HandleFingerUp(Finger lostFinger)
    {
        if (lostFinger != movementFinger || !isInputEnabled) return;

        ResetJoystick();
    }

    private void FixedUpdate()
    {
        if (!isInputEnabled) return;

        Vector3 movement = new Vector3(movementAmount.x, 0, movementAmount.y);
        if (movement != Vector3.zero)
        {
            MovePlayer(movement);
        }
    }

    private void ActivateJoystick(Vector2 startPosition)
    {
        Joystick.gameObject.SetActive(true);
        Joystick.RectTransform.sizeDelta = JoystickSize;
        Joystick.RectTransform.anchoredPosition = ClampStartPosition(startPosition);
    }

    private void ResetJoystick()
    {
        movementFinger = null;
        if (Joystick != null)
        {
            Joystick.Knob.anchoredPosition = Vector2.zero;
            Joystick.gameObject.SetActive(false);
        }
        movementAmount = Vector2.zero;
    }

    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        float x = Mathf.Clamp(startPosition.x, JoystickSize.x / 2, Screen.width - JoystickSize.x / 2);
        float y = Mathf.Clamp(startPosition.y, JoystickSize.y / 2, Screen.height - JoystickSize.y / 2);
        return new Vector2(x, y);
    }

    private Vector2 CalculateKnobPosition(Finger movedFinger)
    {
        Vector2 currentPosition = movedFinger.currentTouch.screenPosition;
        Vector2 joystickPosition = Joystick.RectTransform.anchoredPosition;
        float maxMovement = JoystickSize.x / 2f;

        Vector2 offset = currentPosition - joystickPosition;
        if (offset.magnitude > maxMovement)
        {
            offset = offset.normalized * maxMovement;
        }

        return offset;
    }

    private void MovePlayer(Vector3 movement)
    {
        PlayerRb.MovePosition(transform.position + movement * speed * Time.deltaTime);
        Quaternion targetRotation = Quaternion.LookRotation(movement);
        PlayerRb.MoveRotation(targetRotation);
    }

    private void DisableInput()
    {
        isInputEnabled = false;
        ResetJoystick();
    }

    private void EnableInput()
    {
        isInputEnabled = true;
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

        if (movementFinger != null)
        {
            DisplayFingerPositions(labelStyle);
        }
        else
        {
            GUI.Label(new Rect(10, 35, 500, 20), "No Current Movement Touch", labelStyle);
        }

        DisplayScreenSize(labelStyle);
    }

    private void DisplayFingerPositions(GUIStyle labelStyle)
    {
        GUI.Label(new Rect(10, 35, 500, 20), $"Finger Start Position: {movementFinger.currentTouch.startScreenPosition}", labelStyle);
        GUI.Label(new Rect(10, 65, 500, 20), $"Finger Current Position: {movementFinger.currentTouch.screenPosition}", labelStyle);
    }

    private void DisplayScreenSize(GUIStyle labelStyle)
    {
        GUI.Label(new Rect(10, 10, 500, 20), $"Screen Size ({Screen.width}, {Screen.height})", labelStyle);
    }
}
