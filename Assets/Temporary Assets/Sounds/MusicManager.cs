using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //public AudioSource Track1, Track2, BossMusic, VictoryMusic;
    // public AudioClip StartClip;
    // public AudioClip[] Clip;
    public AudioSource[] clip;
    public int currentClip = 0;
    public int previousClip = -1;
    public float Volume = 1.0f;
    public bool lockTrack = false;
    private bool TrackPlaying = false;
    private bool fade = false;
    private float fadeTime = 0.0f;
    private float TotalTime = 0.0f;
    public void FirstTrack(int startTrack)
    {
        for (int i = 0; i < clip.Length; i++)
        {
            clip[i].volume = 0.0f;
        }

        clip[startTrack].Play();
        currentClip = startTrack;
        previousClip = -1;
        fade = true;
        fadeTime = 20.0f;
        TotalTime = 20.0f;
    }

    public void FadeTrack(int clipNumber, float Time) 
    {
        if (lockTrack){
            return;
        }

        if (currentClip == clipNumber){
            return;
        }

        for (int i = 0; i < clip.Length; i++){
            if (clipNumber == i){
                clip[i].Play();
            }
            else if (i != currentClip) {
                clip[i].Stop();
            }
        }

        previousClip = currentClip;
        currentClip = clipNumber;

        
        fade = true;
        fadeTime = Time;
        TotalTime = Time;
    }

    void Update()
    {
        if (fade) 
        {
            fadeTime -= Time.deltaTime;
            if (fadeTime <= 0.0f)
            {
                fade = false;
                if (previousClip != -1){
                    clip[previousClip].Stop();
                }
            }

            else
            {
                clip[currentClip].volume = (1.0f - fadeTime/TotalTime) * Volume;
                if (previousClip != -1){
                    clip[previousClip].volume = (fadeTime / TotalTime) * Volume; 
                }
                
            }

        }
    }
}
