using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeDatabases();
        InitializeManagers();
        DebugInitialize();
    }

    private void InitializeDatabases()
    {

    }

    private void InitializeManagers()
    {
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void DebugInitialize()
    {

    }
}
