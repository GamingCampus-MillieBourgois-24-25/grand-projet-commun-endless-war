using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    public static GamePauseManager Instance;

    public bool IsGamePaused { get; private set; }

    public event System.Action OnGamePaused;
    public event System.Action OnGameResumed;

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
    }

    public void PauseGame()
    {
        if (!IsGamePaused)
        {
            IsGamePaused = true;
            OnGamePaused?.Invoke();
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        if (IsGamePaused)
        {
            IsGamePaused = false;
            OnGameResumed?.Invoke();
            Time.timeScale = 1f;
        }
    }
}
