using UnityEngine;

public class JoystickConnector : MonoBehaviour
{
    [SerializeField] private FloatingJoyStick movementJoystick;
    [SerializeField] private FloatingJoyStick rotationJoystick;

    private void OnEnable()
    {
        PlayerEvents.OnPlayerSpawned += AssignJoysticks;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerSpawned -= AssignJoysticks;
    }

    private void AssignJoysticks(GameObject player)
    {
        PlayerInput input = player.GetComponent<PlayerInput>();
        if (input == null)
        {
            Debug.LogError("Player prefab has no PlayerInput component!");
            return;
        }

        // Assign joysticks to player
        input.AssignJoysticks(movementJoystick, rotationJoystick);
    }
}
