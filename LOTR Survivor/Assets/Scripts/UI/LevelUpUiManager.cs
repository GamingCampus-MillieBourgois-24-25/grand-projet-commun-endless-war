using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUiManager : MonoBehaviour
{
    public static LevelUpUiManager Instance;

    [Header("Paramètre")]
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private Button buffButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        uiPanel.SetActive(false);
        buffButton.onClick.AddListener(OnBuffChosen);
    }

    public void ShowBuffUi()
    {
        Time.timeScale = 0f;
        uiPanel.SetActive(true);
    }

    private void OnBuffChosen()
    {
        uiPanel.SetActive(false);
        Time.timeScale = 1f;
        XPManager.Instance.OnLevelUpBuffSelected();
    }
}
