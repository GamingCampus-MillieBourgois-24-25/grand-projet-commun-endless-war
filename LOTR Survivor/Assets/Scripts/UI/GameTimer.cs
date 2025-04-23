using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private int minutes = 0;
    private int seconds = 0;
    private float timer = 0f;
    private bool isRunning = true;

    private void Update()
    {
        if (!isRunning) return;

        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            seconds++;
            timer = 0f;

            if (seconds >= 60)
            {
                minutes++;
                seconds = 0;
            }

            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }
}
