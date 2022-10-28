using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBox : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Debris;
    public GameObject PickUp;
    public bool Grounded= false;
    public float health = 1;
    private bool alive = true;
    public void DecreaseHealth(float damage) 
    {
        health -= damage;
        if (health <= 0 && alive) 
        {
            alive = false;
            Instantiate(Debris, this.transform.position, this.transform.rotation);
            //if (PickUp != null) { Instantiate(PickUp, this.transform.position, this.transform.rotation);} 
            if (PickUp != null) {
                if (Grounded) { if (this.GetComponent<Rigidbody>().velocity.y<0.5f) { Instantiate(PickUp, this.transform.position, Quaternion.identity); } }
                else { Instantiate(PickUp, this.transform.position, Quaternion.identity); }
            } 
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
