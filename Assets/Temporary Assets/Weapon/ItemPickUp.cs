using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public GameObject weapon;
    public GameObject TextPrompt;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        { TextPrompt.SetActive(true); }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                PlayerMovementScript player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>();
                    
                player.currentWeapon = weapon.GetComponent<Weapon>();

                weapon.transform.parent = player.hand;
                weapon.transform.localPosition = new Vector3(0.0f,0.0f,0.0f);
                weapon.transform.localRotation = Quaternion.identity;
                Sword sword = weapon.GetComponent<Sword>();
                sword.Player = player;
                sword.anim = player.anim;
                Debug.Log("Item Equiped"); 
            
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        { TextPrompt.SetActive(false); }
    }
}
