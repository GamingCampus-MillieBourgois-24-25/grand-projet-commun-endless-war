using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager Instance;

    [SerializeField] private RectTransform levelUpPanel;
    [SerializeField] private float animationDuration = 0.8f;
    [SerializeField] private float startYOffset = 800f;
    [SerializeField] private Ease animationEase = Ease.OutBounce;
    [SerializeField] private int skillNumber = 4;

    [SerializeField] private GridLayoutGroup layoutGroup;
    [SerializeField] private GameObject skillHolder;
    [SerializeField] private SkillInfo skillInfo;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private GameObject closeButton;

    [SerializeField] private AudioClip levelUpClip;

    private SkillsManager skillManager;
    private SkillSettings currentSkillSettings;

    private Vector2 originalPosition;

    // Singleton pattern
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        originalPosition = levelUpPanel.anchoredPosition;
        levelUpPanel.gameObject.SetActive(false);

        closeButton.SetActive(false);
    }

    private void OnEnable()
    {
        XPEvents.OnLevelUP += DisplayPanel;
        SkillHolderBehaviour.OnSkillSelected += HandleSkillSelected;  // Subscribe to skill selected event
        SkillHolderBehaviour.OnDetailsButton += ShowDetails;
        SkillInfo.OnHide += HideDetails;

        PlayerEvents.OnPlayerSpawned += AssignPlayer; // Listen for when the player spawns
    }

    private void OnDisable()
    {
        XPEvents.OnLevelUP -= DisplayPanel;
        SkillHolderBehaviour.OnSkillSelected -= HandleSkillSelected;  // Unsubscribe from skill selected event
        SkillHolderBehaviour.OnDetailsButton -= ShowDetails;
        SkillInfo.OnHide -= HideDetails;

        PlayerEvents.OnPlayerSpawned -= AssignPlayer; // Unsubscribe from event when disabled
    }

    // This method will be called when the player spawns to assign the skillManager
    private void AssignPlayer(GameObject playerObj)
    {
        skillManager = playerObj.GetComponent<SkillsManager>();

        if (skillManager == null)
            Debug.LogWarning("[LevelUpManager] Player has no SkillsManager!");
        else
            Debug.Log("[LevelUpManager] Player assigned successfully.");
    }

    // This method is called when a skill is selected
    private void HandleSkillSelected(SkillHolderBehaviour selected)
    {
        currentSkillSettings = selected._skillSettings;
        foreach (Transform child in layoutGroup.transform)
        {
            if (child.TryGetComponent(out SkillHolderBehaviour holder))
            {
                if (holder != selected)
                    holder.Unselect();
            }
        }
    }

    // Called when level up occurs to display the level up panel
    public void DisplayPanel(int level)
    {
        if (skillManager == null)
        {
            Debug.LogWarning("[LevelUpManager] skillManager is not assigned, cannot display panel.");
            return;
        }

        VolumeManager.Instance.PlaySFX(levelUpClip, 1f);

        levelUpPanel.gameObject.SetActive(true);
        GamePauseManager.Instance.PauseGame();

        RemoveExistingSkillHolders();
        AddSkillHolders(skillNumber);

        levelUpPanel.DOKill();
        levelUpPanel.anchoredPosition = originalPosition + Vector2.up * startYOffset;

        levelUpPanel.DOAnchorPos(originalPosition, animationDuration)
            .SetEase(animationEase)
            .SetUpdate(true);
    }

    // Hides the level up panel and applies selected skill
    public void HidePanel()
    {
        if (currentSkillSettings == null)
        {
            Debug.LogWarning("[LevelUpManager] No skill selected.");
            return;
        }

        levelUpPanel.DOAnchorPos(originalPosition + Vector2.down * startYOffset, 0.5f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(OnHideComplete);

        ApplySkill(currentSkillSettings);
        currentSkillSettings = null;
    }

    // Resumes the game and triggers level complete event
    private void OnHideComplete()
    {
        GamePauseManager.Instance.ResumeGame();
        levelUpPanel.gameObject.SetActive(false);
        XPEvents.RaiseLevelComplete();
    }

    // Adds skill holders dynamically based on the number
    private void AddSkillHolders(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject holder = Instantiate(skillHolder, layoutGroup.transform);
            holder.name = i == 0 ? "SkillHolder_Starting" : $"SkillHolder_{i}";

            holder.transform.localScale = Vector3.zero;

            float delay = 0.6f + 0.3f * i;
            holder.transform.DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.OutBack)
                .SetDelay(delay)
                .SetUpdate(true);

            SkillSettings skill = null;

            // Select appropriate skill based on index
            if (i == 0)
            {
                skill = SkillLibrary.Instance.GetStartingSkill();
            }
            else if (i == 1)
            {
                skill = SkillLibrary.Instance.GetRandomBuffSkill();
            }
            else
            {
                skill = SkillLibrary.Instance.GetRandomSkill();
            }

            // Update skill data on the holder if skill is available
            if (skill != null && holder.TryGetComponent(out SkillHolderBehaviour behaviour))
            {
                behaviour.UpdateData(skill);
            }
            else
            {
                Debug.LogWarning("Skill is null or component missing for " + holder.name);
            }
        }
    }

    // Removes all skill holders from the UI
    private void RemoveExistingSkillHolders()
    {
        foreach (Transform child in layoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Applies the selected skill to the player
    private void ApplySkill(SkillSettings skill)
    {
        if (skill.skillType != SkillType.Buff)
        {
            skillManager.AddSkill(skill);
        }
        else
        {
            if (skill.buffEffects != null && skill.buffEffects.Length > 0)
            {
                if (skill.buffEffects[0].buffType == BuffType.Heal)
                {
                    ApplyHealingBuff(skill.buffEffects[0].multiplier);
                }
                else
                {
                    foreach (var buff in skill.buffEffects)
                    {
                        PlayerStatsMultiplier.AddBuff(buff.buffType, buff.multiplier);
                    }
                }
            }
            else
            {
                Debug.LogWarning("No buff effects in skill");
            }
        }
    }

    // Applies a healing buff to the player
    private void ApplyHealingBuff(float healMultiplier)
    {
        PlayerHealthBehaviour player = skillManager.GetComponent<PlayerHealthBehaviour>();
        if (player != null)
        {
            player.MaxHealth = Mathf.RoundToInt(player.MaxHealth * healMultiplier);
        }
        else
        {
            Debug.LogWarning("PlayerHealthBehaviour component not found.");
        }
    }

    // Displays skill details when the player selects a skill
    private void ShowDetails(SkillSettings newSkillSettings)
    {
        skillInfo.ShowSkillInfo(newSkillSettings);
        canvasGroup.interactable = false;
        closeButton.SetActive(true);
    }

    // Hides skill details when the player closes them
    private void HideDetails()
    {
        canvasGroup.interactable = true;
        closeButton.SetActive(false);
    }
}
