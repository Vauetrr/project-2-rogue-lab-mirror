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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovementScript>().DecreaseHealth(damage);

        }
        else if (collision.gameObject.tag == "Enemy")
        {
           // collision.gameObject.GetComponent<AiFollow>().DecreaseHealth(damage);
            collision.gameObject.GetComponent<Sorcerer>().DecreaseHealth(damage);


        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
