using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource Track1, Track2;
    public AudioClip StartClip;
    public AudioClip[] Clip;
    public float Volume = 1.0f;
    private bool TrackPlaying =false;
    // Start is called before the first frame update
    void Start()
    {
        //Track1 = gameObject.AddComponent<AudioSource>();
        //Track2 = gameObject.AddComponent<AudioSource>();

        Track1.clip = StartClip;
        Track2.clip = Clip[1];
        Track1.Play();
        Track2.Stop();
        Track1.volume = Volume;
        Track2.volume = Volume;
    }

    public void SwitchTrack(int ClipNumber) 
    {
     
        if (TrackPlaying==false) { Track2.clip = Clip[ClipNumber]; Track1.Stop();Track2.Play(); TrackPlaying = true; }
        else { Track1.clip = Clip[ClipNumber]; Track2.Stop(); Track1.Play(); TrackPlaying = false; }
    }
    // Update is called once per frame
    /*void Update()
    {
        Volume -= 0.00001f;
        Track1.volume = Volume;
    }*/
}
