using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPickUp : MonoBehaviour
{
    public float HP = 10.0f;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("increase HP");
            collision.gameObject.GetComponent<PlayerMovementScript>().DecreaseHealth(-HP);
            Destroy(this.gameObject);
        }
    }
}
