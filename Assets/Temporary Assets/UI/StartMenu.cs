using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] List<GameObject> subMenus;
    [SerializeField] LoadingFade loadingFade;
    [SerializeField] GameObject loadScreen;

    
    public void OnClickPlay()
    {
        StartCoroutine(StartLoad());
    }

    IEnumerator StartLoad(){
        yield return StartCoroutine(loadingFade.StartFade(true, 0.5f));
        ActivateSubmenu(loadScreen);
        SceneManager.LoadScene("tutorial");
    }

    public void ActivateSubmenu(GameObject subMenu){ //can also do it 100% in unity, this is an alt. way
        Debug.Assert(subMenus.Contains(subMenu));

        foreach (var sub in subMenus){
            sub.SetActive(sub == subMenu); // if sub != submenu, will set to false. only sub goes active
        }
    }
}
