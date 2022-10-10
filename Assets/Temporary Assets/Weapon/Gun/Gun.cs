using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun:Weapon{
    //public new weaponEnum weaponType = weaponEnum.Gun;
    public GameObject Projectile;
    private float fireDelay = 0;
    //private GameObject Projectile;
    private float fireSpeed = 3.0f;//100; // delay between attacks. 
                                   // lower value = faster attacks
                                   // modified by Player's attackSpeed

    public override void updateDelay(){
        fireDelay -= Time.deltaTime;
        if (fireDelay < 0){
            fireDelay = 0;
        }
    }
    public override void normalHold(PlayerMovementScript player){

        if (fireDelay <= 0){
            //Instantiate(Projectile, new Vector3(0, 1, 0), Quaternion.identity);
            fireDelay = fireSpeed * player.attackSpeed;
            GameObject o = MonoBehaviour.Instantiate(Projectile, player.ShootLoc.position, Quaternion.identity); // change this to GunProjectile
            o.GetComponent<Rigidbody>().velocity = 30.0f*(player.ShootLoc.position - player.Head.position);
            Debug.Log(Input.mousePosition);
        }
    }

    public override void altAttack(){
        
    }
}