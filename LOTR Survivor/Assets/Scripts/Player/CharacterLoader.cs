using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

public class CharacterLoader : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image characterImage;

    [Header("Gameplay")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    void Awake()
    {
        PlayerStatsSO character = SelectedCharacterData.selectedCharacter;

        if (character == null)
        {
            Debug.LogError("Aucun personnage sélectionné !");
            return;
        }

        characterImage.sprite = character.imageCharacter;

        if (character.characterPrefab != null && spawnPoint != null)
        {
            GameObject instance = Instantiate(character.characterPrefab, spawnPoint.position, Quaternion.identity);

            // On assigne la caméra au bon transform
            virtualCamera.Follow = instance.transform;
        }
        else
        {
            Debug.LogError("Prefab ou spawn point non assigné !");
        }
    }
}
