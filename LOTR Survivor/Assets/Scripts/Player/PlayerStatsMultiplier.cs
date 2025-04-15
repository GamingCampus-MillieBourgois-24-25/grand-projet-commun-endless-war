using System.Collections;
using UnityEngine;

public class PlayerStatsMultiplier : MonoBehaviour
{
    public static float damageMultiplier = 1f;
    public static float speedMultiplier = 1f;
    public static float cooldownMultiplier = 1f;

    private static Coroutine damageCoroutine;
    private static Coroutine speedCoroutine;
    private static Coroutine cooldownCoroutine;

    private static MonoBehaviour runner;

    private void Awake()
    {
        if (runner == null)
            runner = this;
    }

    public static void BuffDamage(float multiplier, float duration)
    {
        if (damageCoroutine != null) runner.StopCoroutine(damageCoroutine);
        damageCoroutine = runner.StartCoroutine(ApplyBuff(() => damageMultiplier = multiplier, () => damageMultiplier = 1f, duration));
    }

    public static void BuffSpeed(float multiplier, float duration)
    {
        if (speedCoroutine != null) runner.StopCoroutine(speedCoroutine);
        speedCoroutine = runner.StartCoroutine(ApplyBuff(() => speedMultiplier = multiplier, () => speedMultiplier = 1f, duration));
    }

    public static void BuffCooldown(float multiplier, float duration)
    {
        if (cooldownCoroutine != null) runner.StopCoroutine(cooldownCoroutine);
        cooldownCoroutine = runner.StartCoroutine(ApplyBuff(() => cooldownMultiplier = multiplier, () => cooldownMultiplier = 1f, duration));
    }

    private static IEnumerator ApplyBuff(System.Action apply, System.Action reset, float duration)
    {
        apply?.Invoke();
        yield return new WaitForSeconds(duration);
        reset?.Invoke();
    }
}
