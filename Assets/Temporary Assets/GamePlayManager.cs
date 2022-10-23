using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public int EnemiesKilled = 0;
    private int EnemiesEngaged = 0;
    public MusicManager musicManager;
    // Start is called before the first frame update
    public void EnemyEngaged() 
    {
       
        musicManager.SwitchTrack(1);
        EnemiesEngaged += 1;
        if (EnemiesEngaged == 1) { musicManager.FadeTrack(1, 1.0f); }//musicManager.SwitchTrack(1); }
    }
    public void EnemyKilled() 
    { 
        EnemiesKilled += 1; 
        EnemiesEngaged -= 1;
        if (EnemiesEngaged == 0) { musicManager.FadeTrack(0, 5.0f); }//musicManager.SwitchTrack(0); } 
    }
    void Start()
    {
        EnemiesKilled = 0;
        EnemiesEngaged = 0;
       
    }

    // Update is called once per frame
    void Update()
    {
        // musicManager.SwitchTrack(1);
    }
}
