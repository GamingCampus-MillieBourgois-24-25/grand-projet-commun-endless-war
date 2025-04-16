using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Finger = UnityEngine.InputSystem.EnhancedTouch.Finger;

public class PlayerInput : MonoBehaviour
{
    [Header("Joystick Setup")]
    [SerializeField] private Vector2 joystickSize = new Vector2(100, 100);
    [SerializeField] private FloatingJoyStick movementJoystick;
    [SerializeField] private FloatingJoyStick rotationJoystick;

    [Header("Player Settings")]
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Finger movementFinger;
    private Finger rotationFinger;

    private Vector2 movementInput;
    private Vector2 rotationInput;

    private bool isInputEnabled = true;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnEnable()
    {
        TouchSimulation.Enable();
        Touch.onFingerDown += HandleFingerDown;
        Touch.onFingerMove += HandleFingerMove;
        Touch.onFingerUp += HandleFingerUp;

        GamePauseManager.Instance.OnGamePaused += DisableInput;
        GamePauseManager.Instance.OnGameResumed += EnableInput;
    }

    private void OnDisable()
    {
        Touch.onFingerDown -= HandleFingerDown;
        Touch.onFingerMove -= HandleFingerMove;
        Touch.onFingerUp -= HandleFingerUp;
        TouchSimulation.Disable();

        GamePauseManager.Instance.OnGamePaused -= DisableInput;
        GamePauseManager.Instance.OnGameResumed -= EnableInput;

        ResetJoystick(movementJoystick);
        ResetJoystick(rotationJoystick);
    }

    private void HandleFingerDown(Finger finger)
    {
        if (!isInputEnabled) return;

        Vector2 pos = finger.screenPosition;

        if (pos.x < Screen.width / 2f && movementFinger == null)
        {
            movementFinger = finger;
            movementInput = Vector2.zero;
            ActivateJoystick(movementJoystick, pos);
        }
        else if (pos.x >= Screen.width / 2f && rotationFinger == null)
        {
            rotationFinger = finger;
            rotationInput = Vector2.zero;
            ActivateJoystick(rotationJoystick, pos);
        }
    }

    private void HandleFingerMove(Finger finger)
    {
        if (!isInputEnabled) return;

        if (finger == movementFinger)
        {
            Vector2 knob = CalculateKnobPosition(finger, movementJoystick);
            movementJoystick.Knob.anchoredPosition = knob;
            movementInput = knob / (joystickSize.x / 2f);
        }
        else if (finger == rotationFinger)
        {
            Vector2 knob = CalculateKnobPosition(finger, rotationJoystick);
            rotationJoystick.Knob.anchoredPosition = knob;
            rotationInput = knob.normalized;
        }
    }

    private void HandleFingerUp(Finger finger)
    {
        if (finger == movementFinger)
        {
            movementFinger = null;
            movementInput = Vector2.zero;
            ResetJoystick(movementJoystick);
        }

        if (finger == rotationFinger)
        {
            rotationFinger = null;
            rotationInput = Vector2.zero;
            ResetJoystick(rotationJoystick);
        }
    }

    private void FixedUpdate()
    {
        if (!isInputEnabled) return;

        Vector3 moveDir = new Vector3(movementInput.x, 0, movementInput.y);
        playerRb.MovePosition(transform.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            playerRb.MoveRotation(Quaternion.Slerp(playerRb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
        else if (rotationInput != Vector2.zero)
        {
            Vector3 lookDir = new Vector3(rotationInput.x, 0, rotationInput.y);
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            playerRb.MoveRotation(Quaternion.Slerp(playerRb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    private void ActivateJoystick(FloatingJoyStick joystick, Vector2 screenPosition)
    {
        joystick.gameObject.SetActive(true);
        joystick.RectTransform.sizeDelta = joystickSize;
        joystick.RectTransform.anchoredPosition = ClampStartPosition(screenPosition);
    }

    private void ResetJoystick(FloatingJoyStick joystick)
    {
        if (joystick != null)
        {
            joystick.Knob.anchoredPosition = Vector2.zero;
            joystick.gameObject.SetActive(false);
        }
    }

    private Vector2 ClampStartPosition(Vector2 screenPosition)
    {
        float x = Mathf.Clamp(screenPosition.x, joystickSize.x / 2, Screen.width - joystickSize.x / 2);
        float y = Mathf.Clamp(screenPosition.y, joystickSize.y / 2, Screen.height - joystickSize.y / 2);
        return new Vector2(x, y);
    }

    private Vector2 CalculateKnobPosition(Finger finger, FloatingJoyStick joystick)
    {
        Vector2 currentPos = finger.currentTouch.screenPosition;
        Vector2 joystickPos = joystick.RectTransform.anchoredPosition;
        float maxMovement = joystickSize.x / 2f;

        Vector2 offset = currentPos - joystickPos;
        if (offset.magnitude > maxMovement)
        {
            offset = offset.normalized * maxMovement;
        }

        return offset;
    }

    private void DisableInput()
    {
        isInputEnabled = false;
        ResetJoystick(movementJoystick);
        ResetJoystick(rotationJoystick);
    }

    private void EnableInput()
    {
        isInputEnabled = true;
    }
}
