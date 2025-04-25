using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private Transform playerParent;
    [SerializeField] private Image characterImageUI;
    [SerializeField] private GameObject defaultCharacterPrefab;
    [SerializeField] private PlayerStatsSO defaultCharacterStats;
    [SerializeField] private Sprite defaultCharacterSprite;

    private GameObject characterInstance;

    private void Start()
    {
        // Use fallback if testing without selecting a character
        var prefabToUse = SelectedCharacterData.selectedCharacterPrefab ?? defaultCharacterPrefab;
        var statsToUse = SelectedCharacterData.selectedCharacter ?? defaultCharacterStats;
        var spriteToUse = SelectedCharacterData.selectedCharacterSprite ?? defaultCharacterSprite;

        if (prefabToUse != null && statsToUse != null)
        {
            characterInstance = Instantiate(prefabToUse, playerParent);
            characterInstance.transform.position = Vector3.zero;

            PlayerEvents.OnPlayerSpawned?.Invoke(characterInstance);

            SetupPlayer(statsToUse);
        }
        else
        {
            Debug.LogError("No character selected or fallback prefab/stats not assigned!");
        }

        if (spriteToUse != null && characterImageUI != null)
        {
            characterImageUI.sprite = spriteToUse;
        }
        else
        {
            Debug.LogWarning("Character sprite is missing or Image UI not assigned.");
        }
    }

    private void SetupPlayer(PlayerStatsSO stats)
    {
        if (stats == null) return;

        var playerHealth = characterInstance.GetComponent<PlayerHealthBehaviour>();
        if (playerHealth != null)
            playerHealth.SetHealth(stats.pointsDeVie);

        var playerMovement = characterInstance.GetComponent<PlayerInput>();
        if (playerMovement != null)
            playerMovement.SetSpeed(stats.vitesseDeDeplacement);

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
