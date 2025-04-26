using System.Collections;
using UnityEngine;

public class ConfettiSpawner : MonoBehaviour
{
    [SerializeField] private GameObject confettiPrefab;
    [SerializeField] private RectTransform canvasTransform;
    [SerializeField] private float scale = 10f;
    [SerializeField] private AudioClip clip;

    [SerializeField] private float minInterval = 0.2f;
    [SerializeField] private float maxInterval = 1f;

    private void Start()
    {
        StartCoroutine(SpawnConfettiLoop());
    }

    private IEnumerator SpawnConfettiLoop()
    {
        while (true)
        {
            SpawnConfetti();

            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }

    private void SpawnConfetti()
    {
        Vector2 randomPosition = new Vector2(
            Random.Range(-canvasTransform.rect.width / 2f, canvasTransform.rect.width / 2f),
            Random.Range(-canvasTransform.rect.height / 2f, canvasTransform.rect.height / 2f)
        );

        Vector3 worldPosition = canvasTransform.TransformPoint(randomPosition);

        VolumeManager.Instance.PlaySFX(clip, 0.5f);

        GameObject confetti = Instantiate(confettiPrefab, worldPosition, Quaternion.identity, canvasTransform);
        confetti.transform.localScale = Vector3.one * scale;
        confetti.SetActive(true);
    }

}
