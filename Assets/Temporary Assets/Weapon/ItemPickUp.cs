using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public GameObject weapon;
    public GameObject TextPrompt;
    public GameObject Equip;
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
                PlayerMovementScript player = other.GetComponent<PlayerMovementScript>();

                player.currentWeapon = weapon.GetComponent<Weapon>();

                weapon.transform.parent = player.hand;
                weapon.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                weapon.transform.localRotation = Quaternion.identity;
                Sword sword = weapon.GetComponent<Sword>();
                sword.Player = player;
                sword.anim = player.anim;
                player.Model.GetComponent<FootSteps>().Trigger = sword.SwordTrigger;
                sword.SwordTrigger.GetComponent<SwordHit>().Player = player;
                sword.SwordTrigger.SetActive(false);
                //weapon.GetComponentInChildren<GameObject>().SetActive(false);
                

                Debug.Log("Item Equiped");
                Destroy(Instantiate(Equip),2.0f);
                Destroy(this.transform.parent.gameObject);
            }
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        { TextPrompt.SetActive(false); }
    }
}
