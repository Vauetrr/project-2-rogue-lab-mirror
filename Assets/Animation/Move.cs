using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    public float Speed = 3;

    Animator anim;
    Vector3 move;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anim = GetComponent<Animator>();
        Speed = 3;
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        move = new Vector3(x, 0, z);

        transform.LookAt(transform.position + new Vector3(x, 0, z));
        transform.position += new Vector3(x, 0, z) * Speed * Time.deltaTime;

        UpdateAnim();
    }

    void UpdateAnim()
    {
        anim.SetFloat("Speed", move.magnitude);
    }
}
