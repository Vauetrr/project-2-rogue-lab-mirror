using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAssetGenerator : MonoBehaviour
{
    public GameObject[] Tiles;
    public int[] Weights;
    public int[] DifficultyMultiplier;
    public bool UseRotation = false;
    public int MaxWalkDistance = 40;
    public SocketData Data;

    public bool bossTel= false;
    public GameObject tel;
    
    // Start is called before the first frame update
    void Start()
    {
        if (bossTel) 
        { 
          // Debug.Log(Data.WalkDistance);
          
           GameObject o = Instantiate(tel, this.transform.position, Quaternion.identity);
          // Debug.Log(o);
           o.GetComponentInChildren<Teleport>().Dist = Data.WalkDistance;
           
        }
        int i = 1;
        //Debug.Log(Weights.Length+" "+ DifficultyMultiplier.Length+ " "+ Tiles.Length +" " + Data+ " " + MaxWalkDistance);
        Weights[0] += DifficultyMultiplier[0] * Data.WalkDistance / MaxWalkDistance;
        for (;i<Weights.Length;i++) 
        {
            Weights[i] += Weights[i - 1]+ DifficultyMultiplier[i] * Data.WalkDistance / MaxWalkDistance;
        }
        //Random.Range(0, Tiles.Length);
        int P=Random.Range(0, Weights[Weights.Length-1]);
        i = 0;
        while (P > Weights[i]) { i++;}

        if (UseRotation) { Instantiate(Tiles[i], this.transform.position, this.transform.rotation); }
        else { Instantiate(Tiles[i], this.transform.position, Quaternion.identity); }
        Destroy(this.gameObject);
    }
}
