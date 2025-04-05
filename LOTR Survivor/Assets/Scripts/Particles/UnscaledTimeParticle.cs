using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnscaledTimeParticle : MonoBehaviour
{
    void Update()
    {
        if (Time.timeScale < 0.01f)
        {
            GetComponent<ParticleSystem>().Simulate(Time.unscaledDeltaTime, true, false);
        }
    }
}