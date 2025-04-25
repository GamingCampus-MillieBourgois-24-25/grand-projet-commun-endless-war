using UnityEngine;
using Cinemachine;

public class CameraFollowConnector : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void OnEnable()
    {
        PlayerEvents.OnPlayerSpawned += SetCameraTarget;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerSpawned -= SetCameraTarget;
    }

    private void SetCameraTarget(GameObject player)
    {
        if (virtualCamera == null)
        {
            Debug.LogError("Virtual Camera is not assigned!");
            return;
        }

        virtualCamera.Follow = player.transform;
    }
}
