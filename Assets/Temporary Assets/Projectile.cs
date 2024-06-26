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
            if (collision.gameObject.GetComponent<Sorcerer>()) { collision.gameObject.GetComponent<Sorcerer>().DecreaseHealth(damage); }
            else if (collision.gameObject.GetComponent<AiFollow>()) { collision.gameObject.GetComponent<AiFollow>().DecreaseHealth(damage); }
            else if (collision.gameObject.GetComponent<Knight>()) { collision.gameObject.GetComponent<Knight>().DecreaseHealth(damage); }

        }
        else if (collision.gameObject.tag == "Interactable")
        {
            if (collision.gameObject.GetComponent<BreakBox>()) { collision.gameObject.GetComponent<BreakBox>().DecreaseHealth(damage); }
        }
    }
}
