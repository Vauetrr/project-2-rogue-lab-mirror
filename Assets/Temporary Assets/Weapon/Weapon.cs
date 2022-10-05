using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum weaponEnum {
    Nothing,
    Gun,
    Bow
};

public class Weapon{

    public weaponEnum weaponType = weaponEnum.Nothing;

    public virtual void updateDelay() {
        // there are no attacks, do nothing
    }

    public virtual void normalAttack(PlayerMovementScript player){
        Debug.Log(weaponType);
    }

    public virtual void altAttack(){

    }
}

public class Gun:Weapon{
    public new weaponEnum weaponType = weaponEnum.Gun;
    private double fireDelay = 0;

    private double fireSpeed = 100; // delay between attacks. 
                                    // lower value = faster attacks
                                    // modified by Player's attackModifier

    public override void updateDelay(){
        fireDelay --;
        if (fireDelay < 0){
            fireDelay = 0;
        }
    }
    public override void normalAttack(PlayerMovementScript player){

        if (fireDelay <= 0){
            //Instantiate(Projectile, new Vector3(0, 1, 0), Quaternion.identity);
            fireDelay = fireSpeed * player.attackSpeed;
            GameObject o = MonoBehaviour.Instantiate(player.Projectile, player.ShootLoc.position, Quaternion.identity);
            o.GetComponent<Rigidbody>().velocity = 10.0f*(player.ShootLoc.position - player.Head.position);
            Debug.Log(Input.mousePosition);
        }
    }

    public override void altAttack(){
        
    }

}