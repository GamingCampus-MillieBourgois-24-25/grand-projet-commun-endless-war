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
        // Charger l'état des tooltips
        SaveLoadManager.LoadTooltipState();

        // Associer les actions aux boutons
        saveAndRestartButton.onClick.AddListener(SaveAndRestart);
        resetTooltipsButton.onClick.AddListener(() =>
        {
            var viewer = FindObjectOfType<TooltipStateViewer>();
            viewer?.ResetTooltips();
        });
    }

    private void SaveAndRestart()
    {
        SaveLoadManager.SaveTooltipState(TooltipState.Instance.ToList());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Relance la scène
    }
}
