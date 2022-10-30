using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMiniMap : MonoBehaviour
{
    // Start is called before the first frame updat
    public Transform Player;
    private RectTransform icon;
    private void Start()
    {
       icon = GetComponent<RectTransform>();
    }
    // Update is called once per frame
    void Update()
    {
        
        //icon.position = new Vector3(200+Player.position.x,1000+Player.position.z, -1);
        //icon.position = new Vector3(0,0, -1);
        icon.localPosition = new Vector3(Player.position.x*200/500-75,Player.position.z * 200 / 500 - 95, -1);
    }
}
