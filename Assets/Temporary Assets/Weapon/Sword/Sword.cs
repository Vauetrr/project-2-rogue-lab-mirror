using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    //public new weaponEnum weaponType = weaponEnum.Sword;
    private float AttackDelay = 0;
    //private GameObject Projectile;
    private float AttackSpeed = 2; // delay between attacks. 
                                     // lower value = faster attacks
                                     // modified by Player's attackSpeed
    public GameObject SwordTrigger;
    public Animator SwordAnimator;
    private bool CanAttack = true;
    IEnumerator UpdateDelay()
    {
        yield return new WaitForSeconds(AttackDelay);
        CanAttack = true;
    }

   
    public override void updateDelay(){
        
    }
    public override void normalHold(PlayerMovementScript player)
    {

        //if (AttackDelay <= 0){
        if (CanAttack) { 
            AttackDelay = AttackSpeed * player.attackSpeed;
            CanAttack = false;
            StartCoroutine(UpdateDelay());
            SwordAnimator.SetBool("SwingSword",true);
            SwordTrigger.SetActive(true);
        }
    }

   

    public override void altAttack(){
        
    }
}

