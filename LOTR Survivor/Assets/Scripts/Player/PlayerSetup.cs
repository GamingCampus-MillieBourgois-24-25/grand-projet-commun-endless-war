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
        if (SelectedCharacterData.selectedCharacter != null && SelectedCharacterData.selectedCharacterPrefab != null)
        {

            characterInstance = Instantiate(SelectedCharacterData.selectedCharacterPrefab, playerParent);
            characterInstance.transform.position = Vector3.zero;

            SetupPlayer(SelectedCharacterData.selectedCharacter);
        }
        else
        {
            Debug.LogError("No character selected or prefab is missing!");
        }

        if (SelectedCharacterData.selectedCharacterSprite != null && characterImageUI != null)
        {
            characterImageUI.sprite = SelectedCharacterData.selectedCharacterSprite;
        }
        else
        {
            Debug.LogError("Character sprite is missing or UI Image component is not assigned!");
        }
    }

    private void SetupPlayer(PlayerStatsSO stats)
    {
        if (stats == null)
        {
            Debug.LogError("PlayerStats is null in SetupPlayer!");
            return;
        }

        var playerHealth = GetComponent<PlayerHealthBehaviour>();
        if (playerHealth != null)
        {
            playerHealth.SetHealth(stats.pointsDeVie);
        }

        var playerMovement = GetComponent<PlayerInput>();
        if (playerMovement != null)
        {
            playerMovement.SetSpeed(stats.vitesseDeDeplacement);
        }

        Debug.Log("Player setup complete with: " + stats.characterName);
    }

    private void OnDestroy()
    {
        if (characterInstance != null)
        {
            Destroy(characterInstance);
        }
    }
}
