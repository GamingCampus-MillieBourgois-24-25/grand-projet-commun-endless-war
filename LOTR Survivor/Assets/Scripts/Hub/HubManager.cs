using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    //~~ Start Game ~~\\
    [SerializeField] private Button startButton;
    [SerializeField] private string nextScene;

    //~~ Power-Up ~~\\
    [SerializeField] private TMP_Text textGold;

    //~~ Character Selection ~~\\
    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private Button nextCharacter;
    [SerializeField] private Button previousCharacter;

    [SerializeField] private PlayerDatabaseSO playerDatabase;
    private int currentCharacterIndex = 0;

    //~~ World Selection ~~\\
    [SerializeField] private Image imageWorld;
    [SerializeField] private TMP_Text worldName;
    [SerializeField] private Button nextWorld;
    [SerializeField] private Button previousWorld;

    void Start()
    {
        if (startButton == null)
        {
            Debug.LogError("Start Button n'est pas assigné dans l'inspecteur !");
            return;
        }

        startButton.onClick.AddListener(ChangeScene);
        nextCharacter.onClick.AddListener(NextCharacter);
        previousCharacter.onClick.AddListener(PreviousCharacter);
        nextWorld.onClick.AddListener(NextWorld);
        previousWorld.onClick.AddListener(PreviousWorld);

        if (playerDatabase != null && playerDatabase.allCharacters.Count > 0)
        {
            currentCharacterIndex = 0;
            DisplayCharacter(currentCharacterIndex);
        }
        else
        {
            Debug.LogError("Aucun personnage dans la base de données !");
        }
    }

    void ChangeScene()
    {
        SelectedCharacterData.selectedCharacter = playerDatabase.allCharacters[currentCharacterIndex];
        SceneManager.LoadScene(nextScene);
    }

    void PreviousCharacter()
    {
        if (playerDatabase == null || playerDatabase.allCharacters.Count == 0) return;

        currentCharacterIndex--;
        if (currentCharacterIndex < 0)
            currentCharacterIndex = playerDatabase.allCharacters.Count - 1;

        DisplayCharacter(currentCharacterIndex);
    }

    void NextCharacter()
    {
        if (playerDatabase == null || playerDatabase.allCharacters.Count == 0) return;

        currentCharacterIndex++;
        if (currentCharacterIndex >= playerDatabase.allCharacters.Count)
            currentCharacterIndex = 0;

        DisplayCharacter(currentCharacterIndex);
    }

    void DisplayCharacter(int index)
    {
        var character = playerDatabase.allCharacters[index];

        if (character.imageCharacter == null)
        {
            Debug.LogError($"Aucune image pour le personnage {character.characterName}");
        }

        characterImage.sprite = character.imageCharacter;
        characterName.text = character.characterName;
        textGold.text = $"PV : {character.pointsDeVie} - Classe : {character.classe}";
    }


    void NextWorld()
    {
        // à implémenter
    }

    void PreviousWorld()
    {
        // à implémenter
    }
}
