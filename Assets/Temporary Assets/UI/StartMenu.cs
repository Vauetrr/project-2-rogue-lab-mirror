using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] List<GameObject> subMenus;
    public void OnClickPlay()
    {
        SceneManager.LoadScene("Test");

        // other way:
        // SceneManager.LoadScene(1); // file ->buildscene to see numbers; but numbers suck dont do this

        // menuname.ssetActive(true/false);    
    }

    public void ActivateSubmenu(GameObject subMenu){ //can also do it 100% in unity, this is an alt. way
        Debug.Assert(subMenus.Contains(subMenu));

        foreach (var sub in subMenus){
            sub.SetActive(sub == subMenu); // if sub != submenu, will set to false. only sub goes active
        }
    }
}
