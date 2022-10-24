using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class LavaDeath : MonoBehaviour
{
    
    private GameObject cam;
    public GameObject fire;
   // public GameObject Player;
    
    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera");
            cam.transform.parent = null;
            Instantiate(fire, collider.transform.position, Quaternion.identity);
        }
    }
}
