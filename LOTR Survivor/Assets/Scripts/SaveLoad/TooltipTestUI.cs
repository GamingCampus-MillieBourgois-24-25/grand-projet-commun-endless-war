using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TooltipTestUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button saveAndRestartButton;
    public Button resetTooltipsButton;

    private void Start()
    {
        SaveLoadManager.LoadTooltipState();

        if (saveAndRestartButton == null)
            saveAndRestartButton.onClick.AddListener(SaveAndRestart);

        if (resetTooltipsButton == null)
            resetTooltipsButton.onClick.AddListener(ResetTooltips);
    }

    public void ResetTooltips()
    {
        TooltipState.Instance.ResetTooltips();
        SaveLoadManager.SaveTooltipState(TooltipState.Instance.ToList());
        Debug.Log("Tooltips reset.");
    }

    public void SaveAndRestart()
    {
        SaveLoadManager.SaveTooltipState(TooltipState.Instance.ToList());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Relance la scène
    }
}
