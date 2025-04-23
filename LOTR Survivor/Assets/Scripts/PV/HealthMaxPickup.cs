using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMaxPickup : MonoBehaviour
{
    [SerializeField] private float healthIncreasePercentage = 20f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealthBehaviour playerHealth = other.GetComponent<PlayerHealthBehaviour>();
        if (playerHealth != null )
        {
            playerHealth.IncreaseMaxHealthByPercentage( healthIncreasePercentage );
            Destroy(gameObject);
        }
    }
}
