using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProjectile : MonoBehaviour
{
    public float lifeTime = 10.0f;
    public float damage = 100.0f;
    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            // Do nothing, it's the player's projectile
        }
        else if (collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<AiFollow>().DecreaseHealth(damage);
            Destroy(gameObject, 0);

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}