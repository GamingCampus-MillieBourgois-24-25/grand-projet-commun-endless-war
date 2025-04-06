using UnityEngine;
using System.Collections;

public class SceneTooltip : MonoBehaviour
{
    [SerializeField] private TooltipData[] tips;
    [SerializeField] private PlayerHealthBehaviour healthBehaviour;

    private void Start()
    {
        TooltipManager.Instance.OnTooltipClosed += OnTipClosed;
        StartCoroutine(ShowTooltipsWithDelay());
        healthBehaviour.OnPlayerDeath += HandleDeathTip;
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
            Debug.LogWarning($"Aucun tooltip trouvé avec l'ID : {id}");
            return;
        }

        TooltipManager.Instance.ShowTip(tooltip);
    }

    private void HandleDeathTip()
    {
        StartCoroutine(ShowTipWithDelay(2, 2f));
    }

    private IEnumerator ShowTipWithDelay(int id, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        ShowTip(id);
    }

    public void OnTipClosed()
    {

    }
}
