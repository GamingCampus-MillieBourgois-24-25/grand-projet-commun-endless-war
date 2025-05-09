using UnityEngine;
using System.Collections;

public class SceneTooltip : MonoBehaviour
{
    [SerializeField] private TooltipData[] tips;

    private void Start()
    {
        StartCoroutine(ShowTooltipsWithDelay());
    }

    private void OnEnable()
    {
        if (TooltipManager.Instance != null)
            TooltipManager.Instance.OnTooltipClosed += OnTipClosed;

        HealthEvents.OnPlayerDeath += HandleDeathTip;
        HealthEvents.OnReviveComplete += HandleReviveTip;
        XPEvents.OnXPPicked += HandleXPTip;
        XPEvents.OnLevelUP += HandleLevelTip;
        PVMaxEvents.OnHPObjectPicked += HandlePVMaxTip;
    }

    private void OnDisable()
    {
        if (TooltipManager.Instance != null)
            TooltipManager.Instance.OnTooltipClosed -= OnTipClosed;

        HealthEvents.OnPlayerDeath -= HandleDeathTip;
        HealthEvents.OnReviveComplete -= HandleReviveTip;
        XPEvents.OnXPPicked -= HandleXPTip;
        XPEvents.OnLevelUP -= HandleLevelTip;
        PVMaxEvents.OnHPObjectPicked -= HandlePVMaxTip;
    }

    private IEnumerator ShowTooltipsWithDelay()
    {
        yield return new WaitForSecondsRealtime(1f);

        ShowTip(0);
        ShowTip(1);
    }

    private void ShowTip(int id)
    {
        TooltipData tooltip = System.Array.Find(tips, tip => tip.tooltipID == id);

        if (tooltip == null)
        {
            Debug.LogWarning($"Aucun tooltip trouv� avec l'ID : {id}");
            return;
        }

        TooltipManager.Instance.ShowTip(tooltip);
    }

    private void HandleDeathTip()
    {
        StartCoroutine(ShowTipWithDelay(2, 2f));
    }

    private void HandleXPTip(int xp)
    {
        XPEvents.OnXPPicked -= HandleXPTip;
        StartCoroutine(ShowTipWithDelay(3, 0.5f));
    }

    private void HandleReviveTip(Transform player)
    {
        ShowTip(4);
    }

    private void HandleLevelTip(int level)
    {
        StartCoroutine(ShowTipWithDelay(5, 1f));
    }

    private void HandlePVMaxTip(float percentage)
    {
        PVMaxEvents.OnHPObjectPicked -= HandlePVMaxTip;
        StartCoroutine(ShowTipWithDelay(6, 0.5f));
    }

    private IEnumerator ShowTipWithDelay(int id, float delay = 1f)
    {
        yield return new WaitForSecondsRealtime(delay);
        ShowTip(id);
    }

    public void OnTipClosed()
    {

    }
}
