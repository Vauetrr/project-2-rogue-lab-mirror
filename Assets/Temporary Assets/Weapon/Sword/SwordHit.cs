using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{

    public float SwingDuration=1.0f;
    public float Damage = 20.0f;
    public Animator SwordAnimator;
    // Start is called before the first frame update
    void OnEnable()
    {
        //PlayAnimation();
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    { 
        yield return new WaitForSeconds(SwingDuration);
        //this.enabled = false;
        //yield break;
        SwordAnimator.SetBool("SwingSword", false);
        this.gameObject.SetActive(false);
       
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("Coollide");
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovementScript>().DecreaseHealth(Damage);
            //Destroy(gameObject, 0);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<AiFollow>().DecreaseHealth(Damage);
            //Destroy(gameObject, 0);
        }
        else if (collision.gameObject.tag == "Interactable")
        {
            collision.gameObject.GetComponent<BreakBox>().DecreaseHealth(Damage);
            //Destroy(gameObject, 0);
        }
    }

}
