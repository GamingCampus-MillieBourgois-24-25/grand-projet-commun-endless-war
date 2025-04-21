using UnityEngine;

public class AttackFXAutoDestroy : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] Animator animator;
    private float animationLength;

    private void OnEnable()
    {
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos.Length > 0)
        {
            animationLength = clipInfos[0].clip.length;
            Destroy(parent, animationLength);
        }
        else
        {
            Debug.LogWarning("No animation clip found on animator.");
        }
    }
}
