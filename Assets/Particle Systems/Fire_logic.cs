using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_logic : MonoBehaviour
{
    void Start()
    {
        ParticleSystem[] particles = this.GetComponentsInChildren<ParticleSystem>();
        StartCoroutine(Speed(particles));
    }

    IEnumerator Speed(ParticleSystem[] particles)
    {
        for (var speed = 3.0f; speed > 1.0f; speed -= (Time.deltaTime/2.0f))
        {
            foreach (var particle in particles)
            {
                var p = particle.main;
                p.simulationSpeed = speed;
            }
            yield return null;
        }
    }
}
