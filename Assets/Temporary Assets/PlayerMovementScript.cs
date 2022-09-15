using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private Rigidbody Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log(Input.mousePosition);
        }
        Vector3 Left = new Vector3( 5.0f, 0.0f, 0.0f );
        Vector3 Forward = new Vector3( 0.0f, 0.0f, 5.0f );

        // Player.AddForce( Input.GetAxis("Horizontal")*Left +  Input.GetAxis("Vertical")*Forward);
        Player.velocity = Input.GetAxis("Horizontal") * Left + Input.GetAxis("Vertical") * Forward;
    }
}
