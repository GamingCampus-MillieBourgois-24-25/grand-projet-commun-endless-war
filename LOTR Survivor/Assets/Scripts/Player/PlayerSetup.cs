using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private Transform playerParent;
    [SerializeField] private Image characterImageUI;
    private GameObject characterInstance;

    private void Start()
    {
        // Check if player already exists in the scene
        if (characterInstance == null)
        {
            if (SelectedCharacterData.selectedCharacter != null && SelectedCharacterData.selectedCharacterPrefab != null)
            {
                characterInstance = Instantiate(SelectedCharacterData.selectedCharacterPrefab, playerParent);
                characterInstance.transform.position = Vector3.zero;

                PlayerEvents.OnPlayerSpawned?.Invoke(characterInstance);

                SetupPlayer(SelectedCharacterData.selectedCharacter);
            }
            else
            {
                Debug.LogError("No character selected or prefab is missing!");
            }

            // Set character sprite in the UI
            if (SelectedCharacterData.selectedCharacterSprite != null && characterImageUI != null)
            {
                characterImageUI.sprite = SelectedCharacterData.selectedCharacterSprite;
            }
            else
            {
                Debug.LogError("Character sprite is missing or UI Image component is not assigned!");
            }
        }
        else
        {
            Debug.Log("Player already exists in the scene. Skipping character setup.");
        }
    }

    private void SetupPlayer(PlayerStatsSO stats)
    {
        if (stats == null)
        {
            Debug.LogError("PlayerStats is null in SetupPlayer!");
            return;
        }

        var playerHealth = characterInstance.GetComponent<PlayerHealthBehaviour>();
        if (playerHealth != null)
        {
            playerHealth.SetHealth(stats.pointsDeVie);
        }
        else
        {
            Debug.LogError("PlayerHealthBehaviour not found on character instance.");
        }

        var playerMovement = characterInstance.GetComponent<PlayerInput>();
        if (playerMovement != null)
        {
            playerMovement.SetSpeed(stats.vitesseDeDeplacement);
        }
        else
        {
            Debug.LogError("PlayerInput not found on character instance.");
        }

        Debug.Log("Player setup complete with: " + stats.characterName);
    }

    private void OnDestroy()
    {
        // Ensure we don't try to destroy the player if it wasn't instantiated
        if (characterInstance != null)
        {
            Destroy(characterInstance);
        }
    }
}
