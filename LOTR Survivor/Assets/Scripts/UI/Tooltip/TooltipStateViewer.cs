using System.Collections.Generic;
using UnityEngine;


public class TooltipStateViewer : MonoBehaviour//Du caca :D
{
    [Header("Current Tooltips")]
    [SerializeField]
    private List<TooltipEntry> currentState = new();

    private static TooltipStateViewer instance;
    private TooltipState state;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        state = TooltipState.Instance;
        currentState = state.ToList();
    }

    private void Update()
    {
        currentState = state.ToList();
    }

    public List<TooltipEntry> GetCurrentState()
    {
        return currentState;
    }

    public void ResetTooltips()
    {
        state.ResetTooltips();
        currentState = state.ToList();
    }
}
