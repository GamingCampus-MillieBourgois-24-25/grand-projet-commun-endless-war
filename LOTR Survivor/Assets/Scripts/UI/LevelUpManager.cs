using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

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
    [SerializeField] private SkillsManager skillManager;
    [SerializeField] private SkillInfo skillInfo;
    [SerializeField] private CanvasGroup canvasGroup;

    private SkillSettings currentSkillSettings;

    private Vector2 originalPosition;

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
        skillManager = GameObject.FindGameObjectWithTag("Player").GetComponent<SkillsManager>();
    }

    private void OnEnable()
    {
        XPEvents.OnLevelUP += DisplayPanel;
        SkillHolderBehaviour.OnSkillSelected += HandleSkillSelected;
        SkillHolderBehaviour.OnDetailsButton += ShowDetails;
        SkillInfo.OnHide += HideDetails;
    }

    private void OnDisable()
    {
        XPEvents.OnLevelUP -= DisplayPanel;
        SkillHolderBehaviour.OnSkillSelected -= HandleSkillSelected;
        SkillHolderBehaviour.OnDetailsButton -= ShowDetails;
        SkillInfo.OnHide -= HideDetails;
    }

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

    public void DisplayPanel(int level)
    {
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

    public void HidePanel()
    {
        if (currentSkillSettings == null)
        {
            return;
        }

        levelUpPanel.DOAnchorPos(originalPosition + Vector2.down * startYOffset, 0.5f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(OnHideComplete);
        ApplySkill(currentSkillSettings);
        currentSkillSettings = null;
    }

    private void OnHideComplete()
    {
        GamePauseManager.Instance.ResumeGame();
        levelUpPanel.gameObject.SetActive(false);
        XPEvents.RaiseLevelComplete();
    }

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

    private void RemoveExistingSkillHolders()
    {
        foreach (Transform child in layoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ApplySkill(SkillSettings skill)
    {
        if (skill.attackSettings.skillType == SkillType.Attack || skill.attackSettings.skillType == SkillType.Starting || skill.attackSettings.skillType == SkillType.Buff)
        {
            skillManager.AddSkill(skill);
        }
        else if (skill.attackSettings.skillType == SkillType.Heal)
        {
            PlayerHealthBehaviour player = skillManager.GetComponent<PlayerHealthBehaviour>();
            player.MaxHealth = Mathf.RoundToInt(player.MaxHealth * skill.attackSettings.HealthBoost);
        }
    }

    private void ShowDetails(SkillSettings newSkillSettings)
    {
        skillInfo.ShowSkillInfo(newSkillSettings);
        canvasGroup.interactable = false;
    }

    private void HideDetails()
    {
        canvasGroup.interactable = true;
    }
}
