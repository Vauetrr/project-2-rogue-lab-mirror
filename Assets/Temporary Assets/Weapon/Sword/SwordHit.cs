using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{

   // public float SwingDuration=1.0f;
    public float Damage = 20.0f;
    public PlayerMovementScript Player;
    private float manaRecharge = 10.0f;

    //public AudioClip SwordHitClip;
    //public AudioSource Source;
    // public Animator SwordAnimator;
    // Start is called before the first frame update

    // didn't align with hitbox, so commented out for now
     //void OnEnable()
     //{
         //PlayAnimation();
         //StartCoroutine(Swing());
     //}

     //IEnumerator Swing()
     //{ 
     //    yield return new WaitForSeconds(0.5f);
     //    this.gameObject.SetActive(false);
     //
     //}

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("Coollide");
        if (collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<PlayerMovementScript>().DecreaseHealth(Damage);
            //Destroy(gameObject, 0);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            Player.DecreaseMana(-manaRecharge);
            if (collision.gameObject.GetComponent<Sorcerer>()) { collision.gameObject.GetComponent<Sorcerer>().DecreaseHealth(Damage); }
            else if (collision.gameObject.GetComponent<AiFollow>()) { collision.gameObject.GetComponent<AiFollow>().DecreaseHealth(Damage); }
            else if (collision.gameObject.GetComponent<Knight>()) { collision.gameObject.GetComponent<Knight>().DecreaseHealth(Damage); }
            else if (collision.gameObject.GetComponent<Boss>()) { collision.gameObject.GetComponent<Boss>().DecreaseHealth(Damage); }
            //Source.PlayOneShot(SwordHitClip);
            //this.enabled = false;
            // collision.gameObject.GetComponent<AiFollow>().DecreaseHealth(Damage);
            //Destroy(gameObject, 0);
        }
        else if (collision.gameObject.tag == "Interactable")
        {
            Player.DecreaseMana(-manaRecharge);
            collision.gameObject.GetComponent<BreakBox>().DecreaseHealth(Damage);
            //Destroy(gameObject, 0);
        }
    }

}
