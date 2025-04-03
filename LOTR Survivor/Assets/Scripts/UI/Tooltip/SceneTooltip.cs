using UnityEngine;

public class SceneTooltip : MonoBehaviour
{
    [SerializeField] private TooltipData rulesTip;
    [SerializeField] private TooltipData healthTip;

    private void Start()
    {
        TooltipManager.Instance.OnTooltipClosed += OnTipClosed;
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
