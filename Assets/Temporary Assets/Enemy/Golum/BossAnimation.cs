using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private AudioClip[] StepClips;
    [SerializeField]
    private AudioClip SmashClip;
    private AudioSource audioSource;
    [SerializeField]
    private GameObject ArmTrigger;
   
    [SerializeField]
    private GameObject ShockWave;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 2.0f;
    }

    public void RSwingStart() 
    {
        ArmTrigger.SetActive(true);
    }
    public void RSwingEnd() 
    {
        ArmTrigger.SetActive(false);
    }
    public void Step()
    {

        audioSource.PlayOneShot(GetRandomClip());


    }

    public void Land() {
        audioSource.PlayOneShot(SmashClip);
        //Destroy(Instantiate(ShockWave, ArmTrigger.transform.position, Quaternion.identity),5.0f);
        Destroy(Instantiate(ShockWave, this.transform.position, Quaternion.identity),5.0f);
    
    }

    private AudioClip GetRandomClip()
    {
        return StepClips[UnityEngine.Random.Range(0, StepClips.Length - 1)];
    }


}