using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float Damage = 20.0f;
    // Start is called before the first frame update
    public GameObject Trail;
    void OnEnable()
    {
        Trail.SetActive(false);
        //PlayAnimation();
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.5f);
        Trail.SetActive(true);

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
            //collision.gameObject.GetComponent<BreakBox>().DecreaseHealth(Damage);
            //Destroy(gameObject, 0);
        }
    }
}
