using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Fire_scale : MonoBehaviour
{
    [SerializeField] private float scale = 1.0f;
    private ParticleSystem[] particles;
    private List<float> initialSizes = new List<float>();

    void Start()
    {
        particles = this.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            initialSizes.Add(particle.main.startSizeMultiplier);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int index = 0;
        foreach (ParticleSystem particle in particles)
        {
            var p = particle.main;
            p.startSizeMultiplier = initialSizes[index++] * scale;
        }
    }
}
