using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun:Weapon{
    //public new weaponEnum weaponType = weaponEnum.Gun;
    public GameObject Projectile;
    public float velocityMultiplier = 30.0f;
    private float fireDelay = 0;
    private float fireSpeed = 0.5f;//100; // delay between attacks. 
                                   // lower value = faster attacks
                                   // modified by Player's attackSpeed

    public Gun(){
        normalDownCost = 20.0f; // cost of attack
    }
    
    public override void updateDelay(){
        fireDelay -= Time.deltaTime;
        if (fireDelay < 0){
            fireDelay = 0;
        }
    }

    public override void normalDown(PlayerMovementScript player){

        if (fireDelay <= 0){
            //Instantiate(Projectile, new Vector3(0, 1, 0), Quaternion.identity);
            fireDelay = fireSpeed * player.attackSpeed;
            var rot = player.Head.rotation * Quaternion.Euler(0, 180f, 0);
            GameObject o = MonoBehaviour.Instantiate(Projectile, player.ShootLoc.position, rot);
            o.GetComponent<Rigidbody>().velocity = velocityMultiplier*(player.ShootLoc.position - player.Head.position);
            //Debug.Log(Input.mousePosition);
        }
    }

    public override void altAttack(PlayerMovementScript player){
        
    }

    public override bool attacking(){
        return fireDelay > 0;
    }
}