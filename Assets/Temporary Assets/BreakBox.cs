using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBox : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Debris;
    public GameObject PickUp;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag =="Projectile") {
            Instantiate(Debris, this.transform.position, this.transform.rotation);
            if (PickUp != null) { Instantiate(PickUp, this.transform.position, this.transform.rotation);} 
            Destroy(gameObject); 
        }
    }
    // Update is called once per frame
}
