using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float Size = 1.0f;
    public float ShockWaveSpeed = 4.0f;
    public float Damage = 20.0f;
    // Update is called once per frame
    void Update()
    {
        Size += ShockWaveSpeed*Time.deltaTime;
        this.transform.localScale = new Vector3(Size,Size,Size);

    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovementScript>().DecreaseHealth(Damage);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy");
            
            
            
            if (collision.gameObject.GetComponent<Sorcerer>()) { collision.gameObject.GetComponent<Sorcerer>().DecreaseHealth(Damage*2); }
            else if (collision.gameObject.GetComponent<AiFollow>()) { collision.gameObject.GetComponent<AiFollow>().DecreaseHealth(Damage*2); }
            else if (collision.gameObject.GetComponent<Knight>()) { collision.gameObject.GetComponent<Knight>().DecreaseHealth(Damage*2); }
        }
        else if (collision.gameObject.tag == "Interactable")
        {
            collision.gameObject.GetComponent<BreakBox>().DecreaseHealth(Damage);
        }
    }


}
