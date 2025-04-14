using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

public class CharacterLoader : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image characterImage;
    [SerializeField] private Sprite defaultCharacterSprite;

    [Header("Gameplay")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject defaultCharacterPrefab;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    void Awake()
    {
        PlayerStatsSO character = SelectedCharacterData.selectedCharacter;

        GameObject prefabToSpawn;
        Sprite imageToUse;

        if (character == null)
        {
            Debug.LogWarning("Aucun personnage s�lectionn�, chargement du personnage par d�faut.");
            prefabToSpawn = defaultCharacterPrefab;
            imageToUse = defaultCharacterSprite;
        }
        else
        {
            prefabToSpawn = character.characterPrefab;
            imageToUse = character.imageCharacter;
        }

        // Met � jour l'image
        if (characterImage != null && imageToUse != null)
            characterImage.sprite = imageToUse;

        // Instancie le prefab
        if (prefabToSpawn != null && spawnPoint != null)
        {
            GameObject instance = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
            virtualCamera.Follow = instance.transform;
        }
        else
        {
            Debug.LogError("Prefab par d�faut ou spawn point non assign� !");
        }
    }
}
