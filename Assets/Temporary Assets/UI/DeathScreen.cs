using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class DeathScreen : MonoBehaviour
{

    [SerializeField] private TMP_Text text;

    public void activate()
    {
        SceneManager.LoadScene("Test");
        
    }
}
