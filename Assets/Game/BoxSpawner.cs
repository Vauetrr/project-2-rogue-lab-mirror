using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject box;
    public float cooldown = 1.0f;
    private float time=0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0.0f) { time = cooldown; 
            GameObject o =Instantiate(box,new Vector3( (float)Random.Range(-30,30),20.0f, (float)Random.Range(-30, 30)),Quaternion.identity);
            
            o.GetComponent<Rigidbody>().velocity=Vector3.down*5.0f;
        }

    }
}
