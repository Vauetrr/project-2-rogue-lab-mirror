using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 10.0f;
    public float damage = 10.0f; 
   
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovementScript>().DecreaseHealth(damage);
           
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<AiFollow>().DecreaseHealth(damage);
           
        }
        else if (collision.gameObject.tag == "Interactable")
        {
            collision.gameObject.GetComponent<BreakBox>().DecreaseHealth(damage);
            
        }
        Destroy(gameObject, 0);
    }
 
}
