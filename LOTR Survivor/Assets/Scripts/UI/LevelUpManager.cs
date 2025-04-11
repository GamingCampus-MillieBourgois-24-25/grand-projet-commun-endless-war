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

        levelUpPanel.DOKill();
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
            GameObject holder = Instantiate(skillHolder, layoutGroup.transform);
            holder.name = i == 0 ? "SkillHolder_Starting" : $"SkillHolder_{i}";

            holder.transform.localScale = Vector3.zero;

            float delay = 0.6f + 0.3f * i;
            holder.transform.DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.OutBack)
                .SetDelay(delay)
                .SetUpdate(true);

            SkillSettings skill = i == 0
                ? SkillLibrary.Instance.GetStartingSkill()
                : SkillLibrary.Instance.GetRandomSkill();

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
}
