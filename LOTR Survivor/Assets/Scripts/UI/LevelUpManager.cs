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
    }

    private void OnEnable()
    {
        XPEvents.OnLevelUP += DisplayPanel;
    }

    private void OnDisable()
    {
        XPEvents.OnLevelUP -= DisplayPanel;
    }

    public void DisplayPanel(int level)
    {
        levelUpPanel.gameObject.SetActive(true);
        GamePauseManager.Instance.PauseGame();

        RemoveExistingSkillHolders();
        AddSkillHolders(skillNumber);

        levelUpPanel.anchoredPosition = originalPosition + Vector2.up * startYOffset;

        levelUpPanel.DOAnchorPos(originalPosition, animationDuration)
            .SetEase(animationEase)
            .SetUpdate(true);
    }

    public void HidePanel()
    {
        levelUpPanel.DOAnchorPos(originalPosition + Vector2.down * startYOffset, 0.5f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(OnHideComplete);
    }

    private void OnHideComplete()
    {
        GamePauseManager.Instance.ResumeGame();
        levelUpPanel.gameObject.SetActive(false);
        XPManager.Instance.OnLevelUpBuffSelected();
    }

    private void AddSkillHolders(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject newSkillHolder = Instantiate(skillHolder, layoutGroup.transform);

            newSkillHolder.name = "SkillHolder_" + i;

            SkillSettings randomSkill = SkillLibrary.Instance.GetRandomSkill();

            if (randomSkill != null)
            {
                SkillHolderBehaviour skillHolderBehaviour = newSkillHolder.GetComponent<SkillHolderBehaviour>();

                if (skillHolderBehaviour != null)
                {
                    skillHolderBehaviour.UpdateData(randomSkill);
                }
            }
            else
            {
                Debug.LogWarning("Skill is null, unable to update skill holder.");
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
}
