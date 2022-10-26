using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadGame : MonoBehaviour
{
    public GameObject loading;
    private void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "Player")
        {
            loading.SetActive(true);
            SceneManager.LoadScene("Test");
        }
    }
}
