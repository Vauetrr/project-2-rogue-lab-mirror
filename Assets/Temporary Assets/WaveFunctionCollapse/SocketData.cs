using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketData : MonoBehaviour
{
    //public int TileType;
    public int UpAjacentType;
    public int DownAjacentType;
    
    public int RightZAjacentType;
    public int LeftZAjacentType;
    
    public int RightXAjacentType;
    public int LeftXAjacentType;

    public int UpRotation;
    public int DownRotation;

    public int RightZRotation;
    public int LeftZRotation;

    public int RightXRotation;
    public int LeftXRotation;

    public bool SymmetricUp;
    public bool SymmetricDown;

    public bool SymmetricNorth;
    public bool SymmetricEast;
    public bool SymmetricSouth;
    public bool SymmetricWest;

    public int Weight=1;

   /* public bool WalkableUp;
    public bool WalkableDown;
    public bool WalkableNorth;
    public bool WalkableSouth;
    public bool WalkableEast;
    public bool WalkableWest;*/
}
