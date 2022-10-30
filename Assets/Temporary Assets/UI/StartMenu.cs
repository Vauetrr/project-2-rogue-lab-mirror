using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartMenu : MonoBehaviour
{
    [SerializeField] List<GameObject> subMenus;
    [SerializeField] LoadingFade loadingFade;
    [SerializeField] GameObject loadScreen;
    [SerializeField] GameObject credits;
    [SerializeField] TMP_Text bonusText;
    [SerializeField] Image background;
    [SerializeField] Material bonusMaterial;

    void Start(){
        StartCoroutine(loadingFade.StartFade(false, 0.0f));
        if (GamePlayManager.manager.beatTheGame){
            ActivateSubmenu(credits);
            bonusText.enabled = true;
            background.material = bonusMaterial;
        }
        else {
            bonusText.enabled = false;
        }
    }
    
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
