using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPickUp : MonoBehaviour
{
    public GameObject weapon;
    public GameObject TextPrompt;
    public GameObject Equip;

    public GameObject Projectile;
    public GameObject Explosion;

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

                player.altWeapon = weapon.GetComponent<Weapon>();
                Gun spell = weapon.GetComponent<Gun>();
                spell.Projectile = Projectile;
                player.DecreaseMana(-player.MaxMana);
                Destroy(Instantiate(Equip), 2.0f);
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

