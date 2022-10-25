using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public int EnemiesKilled = 0;
    private int EnemiesEngaged = 0;
    private int Temp = 0;
    //public int E = 0;
    public MusicManager musicManager;
    // Start is called before the first frame update
    public void EnemyEngaged()
    { 
        EnemiesEngaged = EnemiesEngaged+1;
        if (EnemiesEngaged == 1) {musicManager.FadeTrack(1, 1.0f); }
    }
    public void EnemyKilled() 
    {
        EnemiesKilled += 1; 
        EnemiesEngaged = EnemiesEngaged-1;
        if (EnemiesEngaged == 0) { musicManager.FadeTrack(0, 5.0f); }
    }
    void Start()
    {
        EnemiesKilled = 0;
        EnemiesEngaged = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}