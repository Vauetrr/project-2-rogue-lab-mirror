using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum weaponEnum {
    Nothing,
    Gun,
    Bow,
    Sword
};

public class Weapon : MonoBehaviour
{
    // Identifier for weapon type. 
    //public weaponEnum weaponType = weaponEnum.Nothing;

    // if true, locks player view direction during attack
    public bool lockDirectionDuringAttack = false;
    public bool lockMovementDuringAttack = false;

    public float normalDownCost = 0.0f;
    public float normalUpCost = 0.0f;
    public float normalHoldCost = 0.0f;
    // Refresh weapon attack delay, if applicable
    public virtual void updateDelay() {
    }

    // Action for each input.
    // Not all of these inputs/attacks may be used for each weapon!

    // Action when normal attack button is pressed down
    public virtual void normalDown(PlayerMovementScript player){
    } 

    // Action when normal attack button is let go
    public virtual void normalUp(PlayerMovementScript player){
    }

    // Action when normal attack button is held (behaviour different from Down/Up)
    public virtual void normalHold(PlayerMovementScript player){
    }

    // Action for alt attacks.
    public virtual void altAttack(PlayerMovementScript player){
    }

    public virtual bool attacking(){
        return false; // you can't attack in this state
    }

    public virtual void getDirection(PlayerMovementScript player){
    }
}

