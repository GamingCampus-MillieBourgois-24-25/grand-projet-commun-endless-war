using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class HealthMaxPickup : MonoBehaviour
//{
//    [SerializeField] private float healthIncreasePercentage = 20f;

//    private void OnTriggerEnter(Collider other)
//    {
//        PlayerHealthBehaviour playerHealth = other.GetComponent<PlayerHealthBehaviour>();
//        if (playerHealth != null )
//        {
//            playerHealth.IncreaseMaxHealthByPercentage( healthIncreasePercentage );
//            PVMaxEvents.PickHPObject(healthIncreasePercentage);
//            Destroy(gameObject);
//        }
//    }
//}

public class HealthMaxPickup : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject prefab;
    [SerializeField] private SphereCollider capsuleCollider;
    [SerializeField] private Transform cube;
    [SerializeField] private float healthIncreasePercentage = 20f;

    private bool picked = false;

    private void Awake()
    {
        capsuleCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        picked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !picked)
        {
            picked = true;

            PlayerHealthBehaviour playerHealth = other.GetComponent<PlayerHealthBehaviour>();
            if (playerHealth != null)
            {
                playerHealth.IncreaseMaxHealthByPercentage(healthIncreasePercentage);
                PVMaxEvents.PickHPObject(healthIncreasePercentage);
                if (animator != null)
                {
                    animator.SetTrigger("Picked");
                    Destroy(gameObject, 0.2f);
                }
            }
        }
    }


    public void SetValue(float value)
    {
        healthIncreasePercentage = value;
        if (cube != null)
        {
            cube.transform.localScale *= Mathf.Sqrt(value / 20f);
        }
    }
}

