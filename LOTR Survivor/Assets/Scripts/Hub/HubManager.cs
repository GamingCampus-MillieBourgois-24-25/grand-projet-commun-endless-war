using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class HubManager : MonoBehaviour
{
    [Header("Start Game")]
    [SerializeField] private Button startButton;
    [SerializeField] private string nextScene;

    [Header("Power-Up UI")]
    [SerializeField] private TMP_Text textGold;

    [Header("Character Selection")]
    [SerializeField] private RectTransform characterImageTransform;
    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private Button nextCharacter;
    [SerializeField] private Button previousCharacter;
    [SerializeField] private PlayerDatabaseSO playerDatabase;
    private int currentCharacterIndex = 0;

    //[Header("World Selection")]
    //[SerializeField] private Image imageWorld;
    //[SerializeField] private TMP_Text worldName;
    //[SerializeField] private Button nextWorld;
    //[SerializeField] private Button previousWorld;

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

        DisplayCharacter(currentCharacterIndex, true);
    }

    void NextCharacter()
    {
        if (playerDatabase == null || playerDatabase.allCharacters.Count == 0) return;

        currentCharacterIndex++;
        if (currentCharacterIndex >= playerDatabase.allCharacters.Count)
            currentCharacterIndex = 0;

        DisplayCharacter(currentCharacterIndex, false);
    }
    void DisplayCharacter(int index, bool slideFromLeft = true)
    {
        var character = playerDatabase.allCharacters[index];

        if (character.imageCharacter == null)
        {
            Debug.LogError($"Aucune image pour le personnage {character.characterName}");
        }

<<<<<<< Updated upstream
        characterImage.sprite = character.imageCharacter;
        characterName.text = character.characterName;
    }
=======
        characterImageTransform.DOKill();
        characterImage.DOKill();
>>>>>>> Stashed changes

        float exitDir = slideFromLeft ? 1f : -1f;

        Sequence exitSequence = DOTween.Sequence();
        exitSequence.Append(characterImageTransform.DOAnchorPos(new Vector2(exitDir * 300f, 0f), 0.2f))
                    .Join(characterImage.DOFade(0f, 0.2f));

        exitSequence.OnComplete(() =>
        {
            Vector2 startPos = slideFromLeft ? new Vector2(-500f, 0f) : new Vector2(500f, 0f);
            characterImageTransform.anchoredPosition = startPos;
            characterImage.color = new Color(1f, 1f, 1f, 0f);

            characterImage.sprite = character.imageCharacter;
            characterName.text = character.characterName;
            textGold.text = $"PV : {character.pointsDeVie} - Classe : {character.classe}";

            Sequence enterSequence = DOTween.Sequence();
            enterSequence.Append(characterImageTransform
                                    .DOAnchorPos(Vector2.zero, 0.6f)
                                    .SetEase(Ease.OutBack))
                         .Join(characterImage
                                    .DOFade(1f, 0.4f));
        });
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
