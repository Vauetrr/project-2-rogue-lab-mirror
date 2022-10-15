using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProjectile : MonoBehaviour
{
    public float lifeTime = 10.0f;  
    public float damage = 100.0f;

    void Start() 
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            // Do nothing, it's the player's projectile
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<Sorcerer>()) { collision.gameObject.GetComponent<Sorcerer>().DecreaseHealth(damage); }
            else if (collision.gameObject.GetComponent<AiFollow>()) { collision.gameObject.GetComponent<AiFollow>().DecreaseHealth(damage); }
            else if (collision.gameObject.GetComponent<Knight>()) { collision.gameObject.GetComponent<Knight>().DecreaseHealth(damage); }
            Destroy(gameObject, 0);
        }
        else if (collision.gameObject.tag == "Interactable")
        {
            if (collision.gameObject.GetComponent<BreakBox>()) { collision.gameObject.GetComponent<BreakBox>().DecreaseHealth(damage); }
            Destroy(gameObject, 0);
        }
    }
}