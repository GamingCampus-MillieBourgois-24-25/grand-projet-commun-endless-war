using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager Instance;

    [SerializeField] private RectTransform levelUpPanel;
    [SerializeField] private float animationDuration = 0.8f;
    [SerializeField] private float startYOffset = 800f;
    [SerializeField] private Ease animationEase = Ease.OutBounce;

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
}
