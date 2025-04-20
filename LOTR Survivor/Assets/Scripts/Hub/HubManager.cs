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
    [SerializeField] private Button skillTreeButton;

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

    [Header("Player Stats")]
    [SerializeField] private TMP_Text raceText;
    [SerializeField] private TMP_Text classeText;
    [SerializeField] private TMP_Text pvText;
    [SerializeField] private TMP_Text speedText;

    void Start()
    {
        if (startButton == null)
        {
            Debug.LogError("Start Button is not assigned in the inspector!");
            return;
        }

        startButton.onClick.AddListener(ChangeScene);
        skillTreeButton.onClick.AddListener(SkillTree);
        nextCharacter.onClick.AddListener(NextCharacter);
        previousCharacter.onClick.AddListener(PreviousCharacter);

        if (playerDatabase != null && playerDatabase.allCharacters.Count > 0)
        {
            currentCharacterIndex = 0;
            InitCharacterDisplay(currentCharacterIndex);
        }
        else
        {
            Debug.LogError("No character found in the database!");
        }
    }

    void ChangeScene()
    {
        SelectedCharacterData.selectedCharacter = playerDatabase.allCharacters[currentCharacterIndex];
        Loader.Load(Loader.Scene.TestMobile);
    }

    void SkillTree()
    {
        SelectedCharacterData.selectedCharacter = playerDatabase.allCharacters[currentCharacterIndex];
        Loader.Load(Loader.Scene.SkillTree);
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

    void InitCharacterDisplay(int index)
    {
        var character = playerDatabase.allCharacters[index];

        if (character.imageCharacter == null)
        {
            Debug.LogError("No image for character " + character.characterName);
        }

        characterImageTransform.anchoredPosition = Vector2.zero;
        characterImage.color = Color.white;

        characterImage.sprite = character.imageCharacter;
        characterName.text = character.characterName;

        UpdateCharacterStats(character);
    }

    void DisplayCharacter(int index, bool slideFromLeft = true)
    {
        var character = playerDatabase.allCharacters[index];

        if (character.imageCharacter == null)
        {
            Debug.LogError("No image for character " + character.characterName);
        }

        characterImageTransform.DOKill();
        characterImage.DOKill();

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

            UpdateCharacterStats(character);

            Sequence enterSequence = DOTween.Sequence();
            enterSequence.Append(characterImageTransform
                                    .DOAnchorPos(Vector2.zero, 0.6f)
                                    .SetEase(Ease.OutBack))
                         .Join(characterImage
                                    .DOFade(1f, 0.4f));
        });
    }

    void UpdateCharacterStats(PlayerStatsSO character)
    {
        if (character == null)
        {
            Debug.LogError("Character is null in UpdateCharacterStats!");
            return;
        }

        if (raceText == null || classeText == null || pvText == null || speedText == null)
        {
            Debug.LogError("One or more UI text fields are not assigned in the Inspector!");
            return;
        }

        raceText.text = character.race.ToString();
        classeText.text = character.classe.ToString();
        pvText.text = character.pointsDeVie.ToString();
        speedText.text = character.vitesseDeDeplacement.ToString("F1");
    }



    void NextWorld()
    {
        // To implement
    }

    void PreviousWorld()
    {
        // To implement
    }
}
