using UnityEngine;
using System.Collections;

public class SceneTooltip : MonoBehaviour
{
    [SerializeField] private TooltipData[] tips;

    private void Start()
    {
        TooltipManager.Instance.OnTooltipClosed += OnTipClosed;
        StartCoroutine(ShowTooltipsWithDelay());
    }

    private IEnumerator ShowTooltipsWithDelay()
    {
        yield return new WaitForSeconds(1f);

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


    public void OnTipClosed()
    {

    }
}
