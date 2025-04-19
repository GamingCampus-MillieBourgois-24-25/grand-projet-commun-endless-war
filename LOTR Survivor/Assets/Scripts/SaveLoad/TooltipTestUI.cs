//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class TooltipTestUI : MonoBehaviour
//{
//    private void Awake()
//    {
//        SaveLoadManager.LoadTooltipState();
//    }

//    public void ResetTooltips()
//    {
//        TooltipState.Instance.ResetTooltips();
//        SaveLoadManager.SaveTooltipState(TooltipState.Instance.ToList());
//        Debug.Log("Tooltips reset.");
//    }

//    public void SaveAndRestart()
//    {
//        SaveLoadManager.SaveTooltipState(TooltipState.Instance.ToList());
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Relance la scène
//    }
//}
