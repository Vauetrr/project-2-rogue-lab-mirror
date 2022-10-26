using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpellRandomizer : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem Glow;
    
    [SerializeField]
    private ParticleSystem Ember;

    [SerializeField]
    private ParticleSystem GlowExp;
    [SerializeField]
    private ParticleSystem EmberExp;
    [SerializeField]
    private ParticleSystem GlowPro;
    [SerializeField]
    private ParticleSystem EmberPro;

    private ParticleSystem.ColorOverLifetimeModule GlowColor;
    private ParticleSystem.ColorOverLifetimeModule EmberColor;
    private ParticleSystem.ColorOverLifetimeModule GlowExpColor;
    private ParticleSystem.ColorOverLifetimeModule EmberExpColor;
    private ParticleSystem.ColorOverLifetimeModule GlowProColor;
    private ParticleSystem.ColorOverLifetimeModule EmberProColor;
    // Start is called before the first frame update
    void Start()
    {
        GlowColor = Glow.colorOverLifetime;
        EmberColor = Ember.colorOverLifetime;
        GlowExpColor = GlowExp.colorOverLifetime;
        EmberExpColor = EmberExp.colorOverLifetime; 
        GlowProColor = GlowPro.colorOverLifetime;
        EmberProColor = EmberPro.colorOverLifetime;

        Color Color1 = new Color(Random.Range(0,100)/100.0f, Random.Range(0, 100) / 100.0f, Random.Range(0, 100) / 100.0f);
        Color Color2 = new Color(Random.Range(0,100)/100.0f, Random.Range(0, 100) / 100.0f, Random.Range(0, 100) / 100.0f);
        Color Color3 = new Color(Random.Range(0,100)/100.0f, Random.Range(0, 100) / 100.0f, Random.Range(0, 100) / 100.0f);
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[]
        { new GradientColorKey(Color1, GlowColor.color.gradient.colorKeys[0].time),
          new GradientColorKey(Color2, GlowColor.color.gradient.colorKeys[1].time),
          new GradientColorKey(Color3, GlowColor.color.gradient.colorKeys[2].time)
        }, GlowColor.color.gradient.alphaKeys);
        GlowColor.color = grad;
        EmberColor.color = grad;
        GlowExpColor.color = grad;
        EmberExpColor.color = grad;
        GlowProColor.color = grad;
        EmberProColor.color = grad;

        /*GlowColor.color.gradient.SetKeys(new GradientColorKey[] 
        { new GradientColorKey(Color1, GlowColor.color.gradient.colorKeys[0].time), 
          new GradientColorKey(Color2, GlowColor.color.gradient.colorKeys[1].time),
          new GradientColorKey(Color3, GlowColor.color.gradient.colorKeys[2].time)
        }, GlowColor.color.gradient.alphaKeys);
        //GlowColor.color.gradient.colorKeys[0].color = Color1;
        //GlowColor.color.gradient.colorKeys[1].color = Color2;
        //GlowColor.color.gradient.colorKeys[2].color = Color3;

        EmberColor.color.gradient.SetKeys(new GradientColorKey[]
        { new GradientColorKey(Color1, EmberColor.color.gradient.colorKeys[0].time),
          new GradientColorKey(Color2, EmberColor.color.gradient.colorKeys[1].time),
          new GradientColorKey(Color3, EmberColor.color.gradient.colorKeys[2].time)
        }, EmberColor.color.gradient.alphaKeys);*/

        //EmberColor.color.gradient.colorKeys[0].color = Color1;
        //EmberColor.color.gradient.colorKeys[1].color = Color2;
        //EmberColor.color.gradient.colorKeys[2].color = Color3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
