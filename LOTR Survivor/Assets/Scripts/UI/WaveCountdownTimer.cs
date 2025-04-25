using TMPro;
using System.Collections;
using UnityEngine;

public class WaveCountdownTimer : MonoBehaviour
{
    public static WaveCountdownTimer Instance;

    [SerializeField] private TextMeshProUGUI countdownText;

    private Coroutine countdownCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        countdownText.text = "";
    }

    public void DisplayFinalSurviveCountdown(int time)
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);

        countdownCoroutine = StartCoroutine(FinalSurviveCountdownRoutine(time));
    }

    private IEnumerator FinalSurviveCountdownRoutine(int time)
    {
        int remaining = time;

        while (remaining > 0)
        {
            countdownText.text = $"Final Survive: {remaining}s";
            yield return new WaitForSeconds(1f);
            remaining--;
        }

        countdownText.text = "";
    }

    public void StartCountdown(int time)
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);

        countdownCoroutine = StartCoroutine(CountdownRoutine(time));
    }

    private IEnumerator CountdownRoutine(int time)
    {
        int remaining = time;

        while (remaining > 0)
        {
            countdownText.text = $"Next Wave in {remaining}s";
            yield return new WaitForSeconds(1f);
            remaining--;
        }

        countdownText.text = "";
    }
}
