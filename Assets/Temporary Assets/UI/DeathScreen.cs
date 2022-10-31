using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class DeathScreen : MonoBehaviour
{

    [SerializeField] private TMP_Text text;
    [SerializeField] LoadingFade loadingFade;
    [SerializeField] GameObject loadScreen;
    [SerializeField] List<GameObject> subMenus;


    public IEnumerator activate()
    {
        yield return StartCoroutine(loadingFade.StartFade(true, 3.0f));
        ActivateSubmenu(loadScreen);
        SceneManager.LoadScene("Test");
    }

    public IEnumerator winSequence(){
        text.SetText("Victory!");
        text.color = new Color(144/255f, 238/255f, 144/255f);
        yield return StartCoroutine(loadingFade.StartFade(true, 5.0f));
        ActivateSubmenu(loadScreen);
        SceneManager.LoadScene("StartScene");
    }

    public void ActivateSubmenu(GameObject subMenu){ 
        Debug.Assert(subMenus.Contains(subMenu));

        foreach (var sub in subMenus){
            sub.SetActive(sub == subMenu);
        }
    }

}
