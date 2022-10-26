using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimation : MonoBehaviour
{

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void EndStagger() 
    {
        //Debug.Log("Ended");
        //anim.SetBool("Stagger",false);
        anim.SetInteger("Stagger",0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
