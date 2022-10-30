using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayManager : MonoBehaviour
{
    //[SerializeField] TMP_Text expText;
    [SerializeField] TMP_Text levelValue;
    [SerializeField] TMP_Text tutorialText;
    [SerializeField] TMP_Text spText;
    public HealthBar expBar;
    public int EnemiesKilled = 0;
    private int EnemiesEngaged = 0;
    public int level = 1;
    public int exp = 0;
    public int sp = 0;
    public int hpIncrease = 0;
    public int mpIncrease = 0;
    //public int damageIncrease = 0;
    public int staminaIncrease = 0;
    public bool wasInTutorial = true;
    //public int E = 0;
    public MusicManager musicManager;
    // Start is called before the first frame update

    public static GamePlayManager manager;
    void Awake(){
        if (manager == null) {
            manager = this;
            DontDestroyOnLoad(manager);
            expBar.SetHealthBar(exp/(level*3)==0?1:(level*3));
            tutorialText.enabled = false;
            spText.enabled = false;
        }
        else if (manager != this){
            if (manager.wasInTutorial) {
                manager.wasInTutorial = false;
                manager.tutorialText.enabled = true;
            }
            manager.Start();
            manager.musicManager.FadeTrack(0, 5.0f); 
            Destroy(this.gameObject); // there already exists a GPManager
        }
    }

    public void EnemyEngaged()
    { 
        EnemiesEngaged = EnemiesEngaged+1;
        if (EnemiesEngaged == 1) {musicManager.FadeTrack(1, 1.0f); }
    }

    public void gainExp(int value){
        exp += value;
        if (exp >= level * 3){
            exp = 0;
            level += 1;
            levelValue.SetText(level.ToString());
            sp += 1;

            spText.enabled = true;
        }
        expBar.SetHealthBar((float)exp/(level*3));
    }

    public void allocateSP(int option){
        if (sp <= 0){
            return;
        }

        if (option == 1){
            hpIncrease += 1;
            sp--;
            return;
        }

        if (option == 2){
            mpIncrease += 1;
            sp--;
            return;
        }

        if (option == 3){
            staminaIncrease += 1;
            sp--;
            return;
        }

        Debug.Log("Error: Invalid option selected");
        return;
    }
    
    public void EnemyKilled() 
    {
        EnemiesKilled += 1;
        gainExp(1);
        EnemiesEngaged = EnemiesEngaged-1;
        if (EnemiesEngaged == 0) { musicManager.FadeTrack(0, 5.0f); }
    }
    void Start() {
        EnemiesKilled = 0;
        EnemiesEngaged = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)){
            tutorialText.enabled = !tutorialText.enabled;
        }

        if (sp > 0){
            if (Input.GetKeyDown(KeyCode.Alpha1)){
                allocateSP(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)){
                allocateSP(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)){
                allocateSP(3);
            }

            if (sp <= 0){
                spText.enabled = false;
            }
        }

    }
}