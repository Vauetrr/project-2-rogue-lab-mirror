using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloakColor : MonoBehaviour
{
    public Material CloakMat;
    public Color color;
    private void Start()
    {
        RandomColor();
    }
    public void RandomColor() 
    {
        int R = Random.Range(0, 100);
        int G = Random.Range(0, 100);
        int B = Random.Range(0, 100);
        CloakMat.color = new Color(R/255.0f, G / 255.0f, B / 255.0f);
    } 

}
