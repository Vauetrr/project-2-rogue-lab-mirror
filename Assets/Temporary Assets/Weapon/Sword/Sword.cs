using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Sword : Weapon
{
    //public new weaponEnum weaponType = weaponEnum.Sword;
    private float AttackDelay = 0;
    //private GameObject Projectile;
    private float AttackSpeed = 0.7f; // delay between attacks. 
                                     // lower value = faster attacks
                                     // modified by Player's attackSpeed
    public GameObject SwordModel;
    public GameObject SwordTrigger;
    //public Animator SwordAnimator;
    public Animator anim;
    public PlayerMovementScript Player;
    private bool CanAttack = true;
    private bool CanBuffer = true;
    private int currentCombo = 0;
    private float comboRefresh = 0.5f;

    public TMP_Text text;

    IEnumerator AnimationChain()
    {
        yield return new WaitForSeconds(AttackDelay);

        if (currentCombo >= 4){
            currentCombo = 0;
            anim.SetInteger("AttackChain", currentCombo);
            text.SetText(this.currentCombo.ToString());
        }
        CanAttack = true;
    }

    public Sword(){
        lockDirectionDuringAttack = true;
        lockMovementDuringAttack = true;
    }
    
    IEnumerator UpdateDelay()
    {
        int startingCombo = currentCombo;
        yield return new WaitForSeconds(AttackDelay);
        CanAttack = true;
        yield return new WaitForSeconds(comboRefresh);
        if (startingCombo == currentCombo){
            currentCombo = 0;
            anim.SetInteger("AttackChain", currentCombo);
            text.SetText(this.currentCombo.ToString());
        }
        Debug.Log("starting:" + startingCombo + ", current:" + currentCombo);
    }

   
    public override void updateDelay(){

    }

    public override void normalDown(PlayerMovementScript player)
    {
        AttackDelay = AttackSpeed * player.attackSpeed;

        if (!CanAttack){
            return;
        }

        CanAttack = false;
        
        SwordTrigger.SetActive(true);

        currentCombo++;
        
        text.SetText(this.currentCombo.ToString());
        CanAttack = false;
        SwordTrigger.SetActive(false);
        anim.SetInteger("AttackChain", currentCombo);


        StartCoroutine(UpdateDelay());

        StartCoroutine(AnimationChain());

    }

    public override void normalHold(PlayerMovementScript player)
    {

        
    }

   

    public override void altAttack(PlayerMovementScript player){
        
    }

    public override bool attacking(){
        return !CanAttack;
    }

    void update(){
        text.SetText(this.currentCombo.ToString());
    }
}

