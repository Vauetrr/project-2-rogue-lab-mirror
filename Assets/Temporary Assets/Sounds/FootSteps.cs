using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    //[SerializeField]
    //private AudioClip[] clips;
    [SerializeField]
    private AudioClip WoodStepclip;
    [SerializeField]
    private AudioClip RockStepclip;
    [SerializeField]
    private AudioClip Dodgeclip; 
    [SerializeField]
    private AudioClip Deathclip;  
    [SerializeField]
    private AudioClip Attackclip;

    public GameObject Trigger;
    public Transform Ground;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Step()
    {
        //AudioClip clip = GetRandomClip();
        RaycastHit hitInfo;
        Physics.Raycast(Ground.position, Vector3.down, out hitInfo, 0.5f);
        if (hitInfo.collider.tag == "WoodFloor")
        {
            audioSource.PlayOneShot(WoodStepclip);
        }
        else { audioSource.PlayOneShot(RockStepclip); }
        
    }
    public void sprint() //dash/roll 
    {
        audioSource.PlayOneShot(Dodgeclip);
    }

    public void death() 
    {
        audioSource.PlayOneShot(Deathclip);
    }

    //private bool Attacking = false;
    public void attack() 
    {
        Debug.Log("Attack");
        Debug.Log(Trigger);

       // Attacking = !Attacking;
        //if (Attacking)
        //{
        audioSource.PlayOneShot(Attackclip);
        Trigger.SetActive(true);
        //}
    }
    
    /*private AudioClip GetRandomClip()
    {
        return clips[UnityEngine.Random.Range(0, clips.Length-1)];
    }*/
}