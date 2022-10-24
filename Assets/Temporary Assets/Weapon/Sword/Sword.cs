using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    //public new weaponEnum weaponType = weaponEnum.Sword;
    private float AttackDelay = 0;
    //private GameObject Projectile;
    private float AttackSpeed = 2.9f; // delay between attacks. 
                                     // lower value = faster attacks
                                     // modified by Player's attackSpeed
    public GameObject SwordTrigger;
    //public Animator SwordAnimator;
    public Animator anim;
    public PlayerMovementScript Player;
    private bool CanAttack = true;
    IEnumerator AnimationChain()
    {
        anim.SetInteger("AttackChain", 1);
        yield return new WaitForSeconds(1.0f);
        if (Player.AttackChainCounter > 1)
        {
            anim.SetInteger("AttackChain", 2);
            yield return new WaitForSeconds(1.0f);
            if (Player.AttackChainCounter > 2)
            {
                anim.SetInteger("AttackChain", 3);
                yield return new WaitForSeconds(1.0f);
                if (Player.AttackChainCounter > 3)
                {
                    anim.SetInteger("AttackChain", 4);
                }
            }
        }
        anim.SetInteger("AttackChain", 0);
        Player.AttackChainCounter = 0;
        SwordTrigger.SetActive(false);
        CanAttack = true;
    }

    public Sword(){
        lockDirectionDuringAttack = true;
        lockMovementDuringAttack = true;
    }
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
            //StartCoroutine(UpdateDelay());
            //SwordAnimator.SetBool("SwingSword",true);
            SwordTrigger.SetActive(true);
            StartCoroutine(AnimationChain());
           
        }
    }

   

    public override void altAttack(PlayerMovementScript player){
        
    }

    public override bool attacking(){
        return !CanAttack;
    }
}

