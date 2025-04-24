using UnityEngine;

public class AttackFXAutoDestroy : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private float animationLength;

    private void OnEnable()
    {
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);

        if (clipInfos.Length > 0)
        {
            animationLength = clipInfos[0].clip.length;
            animator.Rebind();
            animator.Update(0f);

            Invoke(nameof(Despawn), animationLength);
        }
        else
        {
            Debug.LogWarning("No animation clip found on animator.");
            Despawn();
        }
    }

    private void Despawn()
    {
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.Despawn(gameObject);
        }
        else
        {
            Debug.LogWarning("ObjectPool instance is null. Destroying the object.");
            Destroy(gameObject);
        }
    }
}
