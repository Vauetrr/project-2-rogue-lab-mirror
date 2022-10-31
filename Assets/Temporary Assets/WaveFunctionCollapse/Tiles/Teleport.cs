using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Teleport : MonoBehaviour
{

    public GameObject TextPrompt;
    public GameObject loading;
    public int Dist = 0;

    private void Start()
    {

        GameObject[] list = GameObject.FindGameObjectsWithTag("tel");
        int largest = 0;
        for (int i = 0; i < list.Length; i++)
        {
            if (largest < list[i].GetComponent<Teleport>().Dist) { largest = list[i].GetComponent<Teleport>().Dist; }
        }
        if (Dist != largest) { Destroy(this.transform.parent.gameObject); }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        { TextPrompt.SetActive(true); }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                loading.SetActive(true);
                SceneManager.LoadScene("Boss");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        { TextPrompt.SetActive(false); }
    }
}