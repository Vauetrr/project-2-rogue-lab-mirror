using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAssetGenerator : MonoBehaviour
{
    public GameObject[] Tiles;
    public int[] Weights;
    public int[] DifficultyMultiplier;
    public bool UseRotation = false;

    // Start is called before the first frame update
    void Start()
    {
        int i = 1;
        for (;i<Weights.Length;i++) 
        {
            Weights[i] += Weights[i - 1];
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
