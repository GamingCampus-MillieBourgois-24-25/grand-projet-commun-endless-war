using TMPro;
using System.Collections;
using UnityEngine;

public class WaveCountdownTimer : MonoBehaviour
{
    public static WaveCountdownTimer Instance;

    [SerializeField] private GameObject textContainer;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject countdownContainer;

    private Coroutine countdownCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (countdownContainer != null)
        {
            countdownContainer.SetActive(true);
        }
    }

    public void DisplayFinalSurviveCountdown(int time)
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);

        textContainer.SetActive(true);
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
        textContainer.SetActive(false);
    }

    public void StartCountdown(int time)
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);

        textContainer.SetActive(true);
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
        textContainer.SetActive(false);
    }
}
