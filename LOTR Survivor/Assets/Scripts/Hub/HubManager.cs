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

    [SerializeField] private PlayerDatabaseSO playerDatabase; // Ajout de la database
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

        startButton.onClick.AddListener(GameScene);
        nextCharacter.onClick.AddListener(NextCharacter);
        previousCharacter.onClick.AddListener(PreviousCharacter);
        nextWorld.onClick.AddListener(NextWorld);
        previousWorld.onClick.AddListener(PreviousWorld);

        //  Affiche le 1er personnage au lancement
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

    void GameScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    //~~ CHARACTER ~~\\
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
        characterImage.sprite = character.imageCharacter;
        characterName.text = character.characterName;

        // Optionnel : afficher gold, race, classe, etc.
        textGold.text = "{character.golds}";
    }

    //~~ WORLD ~~\\
    void NextWorld()
    {
        // à implémenter
    }

    void PreviousWorld()
    {
        // à implémenter
    }
}
