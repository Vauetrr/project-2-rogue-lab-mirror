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

        Track1.clip = StartClip;// StartClip;
        Track2.clip = Clip[1];

        //Track2.Stop();
        Track1.volume = 0.0f;//Volume;
        Track2.volume = 0.0f;
        
        Track1.Play();
        Track2.Play();
        FadeTrack(0,20.0f);
    }

    public void SwitchTrack(int ClipNumber) 
    {
     
        if (TrackPlaying==false) { Track2.clip = Clip[ClipNumber]; Track1.Stop();Track2.Play(); TrackPlaying = true; }
        else { Track1.clip = Clip[ClipNumber]; Track2.Stop(); Track1.Play(); TrackPlaying = false; }
    }


    private bool fade = false;
    private float fadeTime = 0.0f;
    private float TotalTime = 0.0f;
    public void FadeTrack(int ClipNumber, float Time) 
    {
        fade = true;
        fadeTime = Time;
        TotalTime = Time;
        if (TrackPlaying == false) { Track2.clip = Clip[ClipNumber];  Track2.Play();}
        else { Track1.clip = Clip[ClipNumber]; Track1.Play(); }
    }
    // Update is called once per frame
    void Update()
    {
        if (fade) 
        {
            fadeTime -= Time.deltaTime;
            if (fadeTime <= 0.0f)
            {
                fade = false;
                if (TrackPlaying == false) { /*Track1.Stop();*/  TrackPlaying = true; }
                else {  /*Track2.Stop();*/ TrackPlaying = false; }
            }
            else
            {
                if (TrackPlaying == false) { Track2.volume =(1.0f- fadeTime/TotalTime)* Volume; Track1.volume = (fadeTime / TotalTime) * Volume; }
                else { Track1.volume = (1.0f - fadeTime/TotalTime)* Volume; Track2.volume = (fadeTime / TotalTime)*Volume; }
            }
        }
        
    }
}
