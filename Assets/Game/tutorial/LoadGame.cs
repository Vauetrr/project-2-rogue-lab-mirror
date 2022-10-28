using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadGame : MonoBehaviour
{
    [SerializeField] GameObject loading;
    [SerializeField] LoadingFade loadingFade;
    private IEnumerator OnTriggerEnter(Collider other)
    {
        if ( other.tag == "Player")
        {
            yield return StartCoroutine(loadingFade.StartFade(true, 0.0f));
            loading.SetActive(true);
            SceneManager.LoadScene("Test");
        }
    }        
        
}
