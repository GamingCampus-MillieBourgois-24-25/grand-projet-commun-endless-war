using UnityEngine;
using System.Collections;

public class SceneTooltip : MonoBehaviour
{
    [SerializeField] private TooltipData rulesTip;
    [SerializeField] private TooltipData healthTip;

    private void Start()
    {
        TooltipManager.Instance.OnTooltipClosed += OnTipClosed;
        StartCoroutine(ShowTooltipsWithDelay());
    }

    private IEnumerator ShowTooltipsWithDelay()
    {
        yield return new WaitForSeconds(1f);

        ShowRulesTip();
        ShowHealthTip();
    }

    private void ShowRulesTip()
    {
        TooltipManager.Instance.ShowTip(rulesTip);
    }

    private void ShowHealthTip()
    {
        TooltipManager.Instance.ShowTip(healthTip);
    }

    public void OnTipClosed()
    {

    }
}
