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

        //musicManager.SwitchTrack(1);
        Debug.LogError(EnemiesEngaged);
        EnemiesEngaged = EnemiesEngaged+1;
        Debug.Log(EnemiesEngaged);
        Debug.Log("Temp"+Temp);
        Temp = EnemiesEngaged;
        if (EnemiesEngaged == 1) {musicManager.FadeTrack(1, 1.0f); }//musicManager.SwitchTrack(1); }
        //if (EnemiesEngaged == 1) { musicManager.SwitchTrack(1); }
    }
    public void EnemyKilled() 
    {
        //Debug.Log("Killed");
        Debug.LogError(EnemiesEngaged);
        EnemiesKilled += 1; 
        EnemiesEngaged = EnemiesEngaged-1;
        Temp = EnemiesEngaged;
        Debug.Log(EnemiesEngaged);

       // Debug.LogError(E);
       // E = E - 1;
       // Debug.LogError(E);
        if (EnemiesEngaged == 0) { musicManager.FadeTrack(0, 5.0f); }//musicManager.SwitchTrack(0); } 
        //if (EnemiesEngaged == 0) {musicManager.SwitchTrack(0); } 
    }
    void Start()
    {
        EnemiesKilled = 0;
        //EnemiesEngaged = 0;
       // E = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("HEre "+EnemiesEngaged);// musicManager.SwitchTrack(1);
        Debug.Log("E"+EnemiesEngaged);// musicManager.SwitchTrack(1);
        Debug.Log("Temp" +Temp);// musicManager.SwitchTrack(1);

    }
}


/*
public int EnemiesEngaged = 0;
private int Temp = 0;

public void EnemyEngaged()
{

    EnemiesEngaged = EnemiesEngaged + 1;
    Temp = EnemiesEngaged;
    Debug.Log(EnemiesEngaged); 
    Debug.Log(Temp);
}
void Update()
{
    Debug.Log(EnemiesEngaged);
    Debug.Log(Temp);
}

*/

//
/*private int EnemiesEngaged = 0;

public void EnemyEngaged()
{
    EnemiesEngaged = EnemiesEngaged + 1;
    Debug.Log(EnemiesEngaged); //outputs 1                                                                //if (EnemiesEngaged == 1) { musicManager.SwitchTrack(1); }
}
public void EnemyKilled()
{
    EnemiesEngaged = EnemiesEngaged - 1;
    Debug.Log(EnemiesEngaged); //outputs 0                                                               //if (EnemiesEngaged == 0) {musicManager.SwitchTrack(0); } 
}
void Start()
{
    EnemiesEngaged = 0;
}

// Update is called once per frame
void Update()
{
    Debug.Log(EnemiesEngaged); // always outputs 0
}*/