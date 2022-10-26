using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolumAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private AudioClip[] StepClips;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void Step()
    {
       
        audioSource.PlayOneShot(GetRandomClip());
       

    }

    private AudioClip GetRandomClip()
    {
        return StepClips[UnityEngine.Random.Range(0, StepClips.Length - 1)];
    }
  
}
