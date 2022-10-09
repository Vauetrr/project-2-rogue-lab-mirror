using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBox : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Debris;
    public GameObject PickUp;
    public float health = 1;
    public void DecreaseHealth(float damage) 
    {
        health -= damage;
        if (health <= 0) 
        {
            Instantiate(Debris, this.transform.position, this.transform.rotation);
            if (PickUp != null) { Instantiate(PickUp, this.transform.position, this.transform.rotation);} 
            Destroy(gameObject); 
        }
    }
   /* void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag =="Projectile") {
            
        }
    }*/
    // Update is called once per frame
}
