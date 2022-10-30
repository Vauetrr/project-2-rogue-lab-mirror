using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
public class WaveFunctionCollapseSlow : MonoBehaviour
{

    public GameObject[] Tiles;
    public int SizeX;
    public int SizeY;
    public int SizeZ;
    public NavMeshSurface surface;

    public GameObject Error;

    public Walkable walk;

    int[] RotationSocketOffset = { 0, 100000, 200000, 300000 };

    int[] TileWeights;
    int[,] Sockets;
    int[,,] TileMap;
    int[,,] Entropy;
    int[,,,] AvailableTiles;
    int TotalTiles;
    int UsedTiles = 0;

    int[,,] CheckedTiles;

    Stack<int[]> WalkStack = new Stack<int[]>();
    Stack<int[]> TempStack = new Stack<int[]>();
    void BFS_WalkTile(int X, int Y, int Z, int Searched)
    {
        if ((X < SizeX) && (X >= 0) && (Y < SizeY) && (Y >= 0) && (Z < SizeZ) && (Z >= 0) && CheckedTiles[X, Y, Z] == 0)
        {

            //Debug.Log(X +", "+ Y + "," + Z);
            CheckedTiles[X, Y, Z] = Searched;
            int Tile = Sockets[0, TileMap[X, Y, Z]] % 100000;
            for (int i = 0; i < walk.ID_Up.Length; i++)
            { if (walk.ID_Up[i] == Tile && walk.WalkableUp[i]) { WalkStack.Push(new int[] { X, Y + 1, Z }); } }

            Tile = Sockets[1, TileMap[X, Y, Z]] % 100000;
            for (int i = 0; i < walk.ID_Down.Length; i++)
            { if (walk.ID_Down[i] == Tile && walk.WalkableDown[i]) { WalkStack.Push(new int[] { X, Y - 1, Z }); } }

            Tile = Sockets[2, TileMap[X, Y, Z]] % 100000;
            for (int i = 0; i < walk.ID_Other.Length; i++)
            { if (walk.ID_Other[i] == Tile && walk.WalkableOther[i]) { WalkStack.Push(new int[] { X, Y, Z + 1 }); } }
            Tile = Sockets[3, TileMap[X, Y, Z]] % 100000;
            for (int i = 0; i < walk.ID_Other.Length; i++)
            { if (walk.ID_Other[i] == Tile && walk.WalkableOther[i]) { WalkStack.Push(new int[] { X, Y, Z - 1 }); } }

            Tile = Sockets[4, TileMap[X, Y, Z]] % 100000;
            for (int i = 0; i < walk.ID_Other.Length; i++)
            { if (walk.ID_Other[i] == Tile && walk.WalkableOther[i]) { WalkStack.Push(new int[] { X + 1, Y, Z }); } }
            Tile = Sockets[5, TileMap[X, Y, Z]] % 100000;
            for (int i = 0; i < walk.ID_Other.Length; i++)
            { if (walk.ID_Other[i] == Tile && walk.WalkableOther[i]) { WalkStack.Push(new int[] { X - 1, Y, Z }); } }

        }

    }
    int[] BFS()
    {

        int[] pos = null;
        int Searched = 1;
        while (WalkStack.Count != 0)
        {
            while (WalkStack.Count != 0)
            {
                TempStack.Push(WalkStack.Pop());
            }
            Searched += 1;
            while (TempStack.Count != 0)
            {
                pos = TempStack.Pop();
                BFS_WalkTile(pos[0], pos[1], pos[2], Searched);
            }
        }
        return pos;
    }

    void UpdateTileR(int X, int Y, int Z, int I)
    { 
        for (int Index = 0; Index < TotalTiles; Index++)
        {
            if ((X + 1 < SizeX) && (AvailableTiles[X + 1, Y, Z, Index] != 0))
            {
                if (Sockets[4, I] == Sockets[5, Index])
                {
                    bool found = false;
                    for (int i = 0; i < TotalTiles; i++)
                    {                       
                     
                        if ((AvailableTiles[X, Y, Z, i] != 0) && (Sockets[4, i] == Sockets[5, Index]))
                        {
                            found = true; break;
                        }
                    }
                    if (found == false)
                    {
                        AvailableTiles[X + 1, Y, Z, Index] = 0;
                        Entropy[X + 1, Y, Z]--;
                        UpdateTileR(X + 1, Y, Z, Index);
                    }

                }
            }
            if ((X > 0) && (AvailableTiles[X - 1, Y, Z, Index] != 0))
            {
                if (Sockets[5, I] == Sockets[4, Index])
                {
                    bool found = false;
                    for (int i = 0; i < TotalTiles; i++)
                    {
                        if ((AvailableTiles[X, Y, Z, i] != 0) && (Sockets[5, i] == Sockets[4, Index]))
                        {
                            found = true; break;
                        }
                    }
                    if (found == false)
                    {
                        AvailableTiles[X - 1, Y, Z, Index] = 0;
                        Entropy[X - 1, Y, Z]--;
                        UpdateTile(X - 1, Y, Z, Index); ;
                    }

                }
            }
            if ((Y + 1 < SizeY) && (AvailableTiles[X, Y + 1, Z, Index] != 0))
            {
                if (Sockets[0, I] == Sockets[1, Index])
                {
                    bool found = false;
                    for (int i = 0; i < TotalTiles; i++)
                    {
                        if ((AvailableTiles[X, Y, Z, i] != 0) && (Sockets[0, i] == Sockets[1, Index]))
                        {
                            found = true; break;
                        }
                    }
                    if (found == false)
                    {
                        AvailableTiles[X, Y + 1, Z, Index] = 0;
                        Entropy[X, Y + 1, Z]--;
                        UpdateTile(X, Y + 1, Z, Index);
                    }

                }
            }
            if ((Y > 0) && (AvailableTiles[X, Y - 1, Z, Index] != 0))
            {
                if (Sockets[1, I] == Sockets[0, Index])
                {
                    bool found = false;
                    for (int i = 0; i < TotalTiles; i++)
                    {
                        if ((AvailableTiles[X, Y, Z, i] != 0) && (Sockets[1, i] == Sockets[0, Index]))
                        {
                            found = true; break;
                        }
                    }
                    if (found == false)
                    {
                        AvailableTiles[X, Y - 1, Z, Index] = 0;
                        Entropy[X, Y - 1, Z]--;
                        UpdateTile(X, Y - 1, Z, Index);
                    }

                }
            }
            if ((Z + 1 < SizeZ) && (AvailableTiles[X, Y, Z + 1, Index] != 0))
            {
                if (Sockets[2, I] == Sockets[3, Index])
                {
                    bool found = false;
                    for (int i = 0; i < TotalTiles; i++)
                    {
                        if ((AvailableTiles[X, Y, Z, i] != 0) && (Sockets[2, i] == Sockets[3, Index]))
                        {
                            found = true; break;
                        }
                    }
                    if (found == false)
                    {
                        AvailableTiles[X, Y, Z + 1, Index] = 0;
                        Entropy[X, Y, Z + 1]--;
                        UpdateTile(X, Y, Z + 1, Index);
                    }

                }
            }
            if ((Z > 0) && (AvailableTiles[X, Y, Z - 1, Index] != 0))
            {
                if (Sockets[3, I] == Sockets[2, Index])
                {
                    bool found = false;
                    for (int i = 0; i < TotalTiles; i++)
                    {
                        if ((AvailableTiles[X, Y, Z, i] != 0) && (Sockets[3, i] == Sockets[2, Index]))
                        {
                            found = true; break;
                        }
                    }
                    if (found == false)
                    {
                        AvailableTiles[X, Y, Z - 1, Index] = 0;
                        Entropy[X, Y, Z - 1]--;
                        UpdateTile(X, Y, Z - 1, Index);
                    }
                }
            }
        }
    }
    void UpdateTile(int X, int Y, int Z, int I)
    {
        for (int Index = 0; Index < TotalTiles; Index++)
        {
            if ((X + 1 < SizeX) && (AvailableTiles[X + 1, Y, Z, Index] != 0))
            {
                if (Sockets[4, I] == Sockets[5, Index])
                {
                    bool found = false;
                    for (int i = 0; i < TotalTiles; i++)
                    {
                        if ((AvailableTiles[X, Y, Z, i] != 0) && (Sockets[4, i] == Sockets[5, Index]))
                        {
                            found = true; break;
                        }
                    }
                    if (found == false)
                    {
                        AvailableTiles[X + 1, Y, Z, Index] = 0;
                        Entropy[X + 1, Y, Z]--;
                        UpdateTile(X + 1, Y, Z, Index);
                    }

                }
            }
            if ((X > 0) && (AvailableTiles[X - 1, Y, Z, Index] != 0))
            {
                if (Sockets[5, I] == Sockets[4, Index])
                {
                    bool found = false;
                    for (int i = 0; i < TotalTiles; i++)
                    {
                 
                        if ((AvailableTiles[X, Y, Z, i] != 0 && I != i) && (Sockets[5, i] == Sockets[4, Index]))
                        {
                            found = true; break;
                        }
                    }
                    if (found == false)
                    {
                        AvailableTiles[X - 1, Y, Z, Index] = 0;
                        Entropy[X - 1, Y, Z]--;
                        UpdateTile(X - 1, Y, Z, Index); ;
                    }

                }
            }
            if ((Y + 1 < SizeY) && (AvailableTiles[X, Y + 1, Z, Index] != 0))
            {
                if (Sockets[0, I] == Sockets[1, Index])
                {
                    bool found = false;
                    for (int i = 0; i < TotalTiles; i++)
                    {
                    
                        if ((AvailableTiles[X, Y, Z, i] != 0 && I != i) && (Sockets[0, i] == Sockets[1, Index]))
                        {
                            found = true; break;
                        }
                    }
                    if (found == false)
                    {
                        AvailableTiles[X, Y + 1, Z, Index] = 0;
                        Entropy[X, Y + 1, Z]--;
                        UpdateTile(X, Y + 1, Z, Index);
                    }

                }
            }
            if ((Y > 0) && (AvailableTiles[X, Y - 1, Z, Index] != 0))
            {
                if (Sockets[1, I] == Sockets[0, Index])
                {
                    bool found = false;
                    for (int i = 0; i < TotalTiles; i++)
                    {
                       
                        if ((AvailableTiles[X, Y, Z, i] != 0 && I != i) && (Sockets[1, i] == Sockets[0, Index]))
                        {
                            found = true; break;
                        }
                    }
                    if (found == false)
                    {
                        AvailableTiles[X, Y - 1, Z, Index] = 0;
                        Entropy[X, Y - 1, Z]--;
                        UpdateTile(X, Y - 1, Z, Index);
                    }

                }
            }
            if ((Z + 1 < SizeZ) && (AvailableTiles[X, Y, Z + 1, Index] != 0))
            {
                if (Sockets[2, I] == Sockets[3, Index])
                {
                    bool found = false;
                    for (int i = 0; i < TotalTiles; i++)
                    {
                       
                        if ((AvailableTiles[X, Y, Z, i] != 0 && I != i) && (Sockets[2, i] == Sockets[3, Index]))
                        {
                            found = true; break;
                        }
                    }
                    if (found == false)
                    {
                        AvailableTiles[X, Y, Z + 1, Index] = 0;
                        Entropy[X, Y, Z + 1]--;
                        UpdateTile(X, Y, Z + 1, Index);
                    }

                }
            }
            if ((Z > 0) && (AvailableTiles[X, Y, Z - 1, Index] != 0))
            {
                if (Sockets[3, I] == Sockets[2, Index])
                {
                    bool found = false;
                    for (int i = 0; i < TotalTiles; i++)
                    {
                        if ((AvailableTiles[X, Y, Z, i] != 0 && I != i) && (Sockets[3, i] == Sockets[2, Index]))
                        {
                            found = true; break;
                        }
                    }
                    if (found == false)
                    {
                        AvailableTiles[X, Y, Z - 1, Index] = 0;
                        Entropy[X, Y, Z - 1]--;
                        UpdateTile(X, Y, Z - 1, Index);
                    }
                }
            }
        }
    }
    void SetTile(int X, int Y, int Z, int I)
    {
        //Instantiate(Error, this.transform.position + new Vector3(X * 20.0f, Y * 20.0f, Z * 20.0f), Quaternion.identity);
    
        UsedTiles++;
        TileMap[X, Y, Z] = I;
        Entropy[X, Y, Z] = 1000000000;
        //int I = TileMap[X, Y, Z];
        for (int Index = 0; Index < TotalTiles; Index++) { AvailableTiles[X, Y, Z, Index] = 0; }
        //AvailableTiles[X, Y, Z, I] = 1;
        for (int Index = 0; Index < TotalTiles; Index++)
        {
            //AvailableTiles[X, Y, Z, Index] = 0;
            if ((X + 1 < SizeX) && (AvailableTiles[X + 1, Y, Z, Index] != 0))
            {
                if (Sockets[4, I] != Sockets[5, Index])
                {
                    AvailableTiles[X + 1, Y, Z, Index] = 0;
                    Entropy[X + 1, Y, Z]--;
                    UpdateTile(X + 1, Y, Z, Index);
                }
            }
            if ((X > 0) && (AvailableTiles[X - 1, Y, Z, Index] != 0))
            {
                if (Sockets[5, I] != Sockets[4, Index])
                {
                    AvailableTiles[X - 1, Y, Z, Index] = 0;
                    Entropy[X - 1, Y, Z]--;
                    UpdateTile(X - 1, Y, Z, Index);
                }
            }
            if ((Y + 1 < SizeY) && (AvailableTiles[X, Y + 1, Z, Index] != 0))
            {
                if (Sockets[0, I] != Sockets[1, Index])
                {
                    AvailableTiles[X, Y + 1, Z, Index] = 0;
                    Entropy[X, Y + 1, Z]--;
                    UpdateTile(X, Y + 1, Z, Index);
                }
            }
            if ((Y > 0) && (AvailableTiles[X, Y - 1, Z, Index] != 0))
            {
                if (Sockets[1, I] != Sockets[0, Index])
                {
                    AvailableTiles[X, Y - 1, Z, Index] = 0;
                    Entropy[X, Y - 1, Z]--;
                    UpdateTile(X, Y - 1, Z, Index);
                }
            }
            if ((Z + 1 < SizeZ) && (AvailableTiles[X, Y, Z + 1, Index] != 0))
            {
                if (Sockets[2, I] != Sockets[3, Index])
                {
                    AvailableTiles[X, Y, Z + 1, Index] = 0;
                    Entropy[X, Y, Z + 1]--;
                    UpdateTile(X, Y, Z + 1, Index);
                }
            }
            if ((Z > 0) && (AvailableTiles[X, Y, Z - 1, Index] != 0))
            {
                if (Sockets[3, I] != Sockets[2, Index])
                {
                    AvailableTiles[X, Y, Z - 1, Index] = 0;
                    Entropy[X, Y, Z - 1]--;
                    UpdateTile(X, Y, Z - 1, Index);
                }
            }
        }
    }

    void Remove3x3x3(int x, int y, int z)
    {
        for (int X = x - 1; X <= x + 1; X++)
        {
            for (int Y = y - 1; Y <= y + 1; Y++)
            {
                for (int Z = z - 1; Z <= z + 1; Z++)
                {
                    TileMap[X, Y, Z] = Tiles.Length * 4;
                    Entropy[X, Y, Z] = TotalTiles;
                    for (int Tile = 0; Tile < TotalTiles; Tile++)
                    {
                        AvailableTiles[X, Y, Z, Tile] = 1;
                    }
                }
            }
        }
    }


    void CalculateEntropy( int x, int z) 
    {
        for (int Y = 0; Y < SizeY; Y++)
        {
            for (int Z = Mathf.Clamp((z - 1), 0, SizeZ - 1); Z <= Mathf.Clamp((z + 1), 0, SizeZ - 1); Z++)
            {

                if (x - 2 > 0 && x + 2 < SizeX)
                {
                    SetTile(x - 2, Y, Z, TileMap[x - 2, Y, Z]);
                    SetTile(x + 2, Y, Z, TileMap[x + 2, Y, Z]);
                }


            }
            for (int X = Mathf.Clamp((x - 1), 0, SizeX - 1); X <= Mathf.Clamp((x - 1), 0, SizeX - 1); X++)
            {
                if (z - 2 > 0 && z + 2 < SizeZ)
                {
                    SetTile(X, Y, z - 2, TileMap[X, Y, z - 2]);
                    SetTile(X, Y, z + 2, TileMap[X, Y, z + 2]);
                }
            }

            /*for (int Z = Mathf.Clamp((z - 2),0,SizeZ-1); Z <= Mathf.Clamp((z + 2), 0, SizeZ-1); Z++)
            {

                if (x - 2 > 0 && x + 2 < SizeX)
                {
                    SetTile(x - 2, Y, Z, TileMap[x - 2, Y, Z]);
                    SetTile(x + 2, Y, Z, TileMap[x + 2, Y, Z]);
                    //UpdateTile(x - 2, Y, Z, TileMap[x - 2, Y, Z]);
                    //UpdateTile(x + 2, Y, Z, TileMap[x + 2, Y, Z]);
                }
                
                
            }
            for (int X = Mathf.Clamp((x - 1), 0, SizeX-1); X <= Mathf.Clamp((x - 1), 0, SizeX-1); X++)
            {
                if (z - 2 > 0 && z + 2 < SizeZ)
                {
                    SetTile(X, Y, z - 2, TileMap[X, Y, z - 2]);
                    SetTile(X, Y, z + 2, TileMap[X, Y, z + 2]);
                    
                    //UpdateTile(X, Y, z - 2, TileMap[X, Y, z - 2]);
                    //UpdateTile(X, Y, z + 2, TileMap[X, Y, z + 2]);
                }
            }*/
        }
        
        
        /*for (int X = x - 2; X <= x + 2; X++)
        {
            
        }*/
    } 
    void Remove3x3(int x, int z)
    {
        for (int X = x - 1; X <= x + 1; X++)
        {
            for (int Y = 0; Y < SizeY; Y++)
            {
                for (int Z = z - 1; Z <= z + 1; Z++)
                {
                    TileMap[X, Y, Z] = Tiles.Length * 4;
                    Entropy[X, Y, Z] = TotalTiles;
                    CheckedTiles[X, Y, Z] = 0;
                    for (int Tile = 0; Tile < TotalTiles; Tile++)
                    {
                        AvailableTiles[X, Y, Z, Tile] = 1;
                    }
                }
            }
        }
    }
    void RemoveAllTiles() 
    {
        for (int X = 0; X <SizeX; X++)
        {
            for (int Y = 0; Y <SizeY; Y++)
            {
                for (int Z = 0; Z <SizeZ; Z++)
                {
                    //TileMap[X, Y, Z] = Tiles.Length * 4;
                    Entropy[X, Y, Z] = TotalTiles;
                    CheckedTiles[X, Y, Z] = 0;
                    for (int Tile = 0; Tile < TotalTiles; Tile++)
                    {
                        AvailableTiles[X, Y, Z, Tile] = 1;
                    }
                }
            }
        }
    }
    private int StartI = 5, StartX = 10, StartY = 4, StartZ = 10;
    void PresetTiles() 
    {
        SetTile(0, 0, 0, 1);
        SetTile(1, 0, 0, 1);
        //for (int x = 5; x < SizeX; x++) { SetTile(x, 0, 0, 1); }
        //for (int x = 0; x < SizeX; x++) { SetTile(x, 0, SizeZ - 1, 1); }
        //for (int z = 1; z < SizeZ - 1; z++) { SetTile(0, 0, z, 1); }
        //for (int z = 1; z < SizeZ - 1; z++) { SetTile(SizeX - 1, 0, z, 1); }
        SetTile(3, 4, 0, 2);



        SetTile(SizeX-1, 0, SizeZ-1, 1);
        SetTile(SizeX-2, 0, SizeZ-1, 1);
        SetTile(SizeX-4, 4, SizeZ-1, 2+ Tiles.Length * 2);
        for (int x = 5; x < SizeX; x++) { SetTile(x, 0, 0, 1); }
        for (int x = 0; x < SizeX-6; x++) { SetTile(x, 0, SizeZ - 1, 1); }
        for (int z = 1; z < SizeZ - 1; z++) { SetTile(0, 0, z, 1); }
        for (int z = 1; z < SizeZ - 1; z++) { SetTile(SizeX - 1, 0, z, 1); }

        for (int y = 1; y < SizeY; y++)
        {
            for (int x = 5; x < SizeX; x++) { SetTile(x, y, 0, Tiles.Length * 4); }
            for (int x = 0; x < SizeX - 6; x++) { SetTile(x, y, SizeZ - 1, Tiles.Length * 4); }
            for (int z = 1; z < SizeZ - 1; z++) { SetTile(0, y, z, Tiles.Length * 4); }
            for (int z = 1; z < SizeZ - 1; z++) { SetTile(SizeX - 1, y, z, Tiles.Length * 4); }
        }

        //for (int x = 4; x < SizeX - 4; x++) {
        //    for (int z = 4; z < SizeZ - 4; z++) { SetTile(x, 0, z, Tiles.Length * 4 + 1); }
        //}
       

    }
    bool SetTiles(int Count) 
    {
        // SizeX * SizeY * SizeZ - UsedTiles
        for (int TileIndex = 0; TileIndex < Count; TileIndex++)
        {
            int lowest = 100000000;
            int X = 0, Y = 0, Z = 0;


            //X = Random.Range(1, SizeX-2); Y = Random.Range(1, SizeY - 2); Z = Random.Range(1, SizeZ - 2);
            //if (Entropy[X, Y, Z] < 10000) { }
            //else
            //{
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    for (int z = 0; z < SizeZ; z++)
                    {
                        if (Entropy[x, y, z] < lowest)//&& Entropy[x,y,z]!=0)
                        {
                            X = x; Y = y; Z = z; lowest = Entropy[x, y, z];
                        }
                        else if (Entropy[x, y, z] == lowest) //Do The Closest First
                        {

                            float Dist = (X - StartX) * (X - StartX) + (Y - StartY) * (Y - StartY) + (Z - StartZ) * (Z - StartZ);
                            float NewDist = (x - StartX) * (x - StartX) + (y - StartY) * (y - StartY) + (z - StartZ) * (z - StartZ);
                            //if (Random.Range(0, 10) > 9)
                            //{
                            if ((NewDist < Dist)) { X = x; Y = y; Z = z; lowest = Entropy[x, y, z]; }
                            //}
                        }
                    }
                }
            }
            //}
            int[] TryTile = new int[TotalTiles];
            int[] TryWeights = new int[TotalTiles];
            int index = 0;

            for (int Tile = 0; Tile < TotalTiles; Tile++)
            {
                if (AvailableTiles[X, Y, Z, Tile] == 1)
                {

                    TryTile[index] = Tile;
                    AvailableTiles[X, Y, Z, Tile] = 0;
                    index++;
                }
            }
            if (index == 0) { return true; }
            //Debug.Log("Find Tile (" + X + ", " + Y + ", " + Z + " ) Entropy[x,y,z] = " + Entropy[X, Y, Z]);

            //if (index == 0 & Error0 == false)
           // { Error0 = true; Debug.LogError("Unable To Find Tile (" + X + ", " + Y + ", " + Z + " ) Entropy[x,y,z] = " + Entropy[X, Y, Z]); Instantiate(Error, this.transform.position + new Vector3(X * 20.0f, Y * 20.0f, Z * 20.0f), Quaternion.identity); }
            /*
            
            int I1 = 1;
            TryWeights[0] = TileWeights[TryTile[0]];
            for (; I1 < index; I1++)
            {
                TryWeights[I1] = TileWeights[TryTile[I1]];
                TryWeights[I1] += TryWeights[I1 - 1];
            }
            //Random.Range(0, Tiles.Length);
           // Debug.Log(index + "  " + TotalTiles+ " Tile (" + X + ", " + Y + ", " + Z + " ) Entropy[x,y,z] = " + Entropy[X, Y, Z]);
            int P = Random.Range(0, TryWeights[index - 1]);
            I1 = 0;
            while (TryWeights[I1]<=P) { I1++; }
            if (index != 1) { Debug.Log(TryWeights[index - 1] + " index " + index + " I1 " +I1+" P "+P +" TryTile "+ TryTile[I1]); }
            */
            SetTile(X,Y,Z, TryTile[Random.Range(0, index)]);
        }
        return false;
    }
    //void resetTiles() { }
    // Start is called before the first frame update
    void Start()
    {
        bool Error0 = false;

        //int[,] Sockets = new int[6, Tiles.Length * 4];
        TotalTiles = (Tiles.Length * 4) + 2;
        Sockets = new int[6, TotalTiles];
        TileWeights = new int[TotalTiles];
        for (int i = 0; i < Tiles.Length; i++)
        {

            TileWeights[i] =                    Tiles[i].GetComponent<SocketData>().Weight;
            TileWeights[i + Tiles.Length] =     Tiles[i].GetComponent<SocketData>().Weight;
            TileWeights[i + 2 * Tiles.Length] = Tiles[i].GetComponent<SocketData>().Weight;
            TileWeights[i + 3 * Tiles.Length] = Tiles[i].GetComponent<SocketData>().Weight;


            int[] Temp = new int[6];
            Temp[0] = Tiles[i].GetComponent<SocketData>().UpAjacentType;
            Temp[1] = Tiles[i].GetComponent<SocketData>().DownAjacentType;
            Temp[2] = Tiles[i].GetComponent<SocketData>().RightZAjacentType;
            Temp[3] = Tiles[i].GetComponent<SocketData>().LeftZAjacentType;
            Temp[4] = Tiles[i].GetComponent<SocketData>().RightXAjacentType;
            Temp[5] = Tiles[i].GetComponent<SocketData>().LeftXAjacentType;

            if (Tiles[i].GetComponent<SocketData>().SymmetricUp)
            {
                Sockets[0, i] = Temp[0];
                Sockets[0, i +     Tiles.Length] = Temp[0];
                Sockets[0, i + 2 * Tiles.Length] = Temp[0];
                Sockets[0, i + 3 * Tiles.Length] = Temp[0];
            }
            else
            {
                Sockets[0, i] = Temp[0] + RotationSocketOffset[Tiles[i].GetComponent<SocketData>().UpRotation];
                Sockets[0, i + Tiles.Length] = Temp[0] + RotationSocketOffset[(1 + Tiles[i].GetComponent<SocketData>().UpRotation) % 4];
                Sockets[0, i + 2 * Tiles.Length] = Temp[0] + RotationSocketOffset[(2 + Tiles[i].GetComponent<SocketData>().UpRotation) % 4];
                Sockets[0, i + 3 * Tiles.Length] = Temp[0] + RotationSocketOffset[(3 + Tiles[i].GetComponent<SocketData>().UpRotation) % 4];
            }
            if (Tiles[i].GetComponent<SocketData>().SymmetricDown)
            {
                Sockets[1, i] = Temp[1];
                Sockets[1, i + Tiles.Length] = Temp[1];
                Sockets[1, i + 2 * Tiles.Length] = Temp[1];
                Sockets[1, i + 3 * Tiles.Length] = Temp[1];
            }
            else
            {
                Sockets[1, i] = Temp[1] + RotationSocketOffset[Tiles[i].GetComponent<SocketData>().DownRotation];
                Sockets[1, i + Tiles.Length] = Temp[1] + RotationSocketOffset[(1 + Tiles[i].GetComponent<SocketData>().DownRotation) % 4];
                Sockets[1, i + 2 * Tiles.Length] = Temp[1] + RotationSocketOffset[(2 + Tiles[i].GetComponent<SocketData>().DownRotation) % 4];
                Sockets[1, i + 3 * Tiles.Length] = Temp[1] + RotationSocketOffset[(3 + Tiles[i].GetComponent<SocketData>().DownRotation) % 4];
            }

            if (Tiles[i].GetComponent<SocketData>().SymmetricNorth)
            {
                Sockets[2, i] = Temp[2];
                Sockets[4, i + Tiles.Length] = Temp[2];
                Sockets[3, i + 2 * Tiles.Length] = Temp[2];
                Sockets[5, i + 3 * Tiles.Length] = Temp[2];
            }
            else
            {
                Sockets[2, i] = Temp[2] + RotationSocketOffset[Tiles[i].GetComponent<SocketData>().RightZRotation];
                Sockets[4, i + Tiles.Length] = Temp[2] + RotationSocketOffset[(1 + Tiles[i].GetComponent<SocketData>().RightZRotation) % 4];
                Sockets[3, i + 2 * Tiles.Length] = Temp[2] + RotationSocketOffset[(2 + Tiles[i].GetComponent<SocketData>().RightZRotation) % 4];
                Sockets[5, i + 3 * Tiles.Length] = Temp[2] + RotationSocketOffset[(3 + Tiles[i].GetComponent<SocketData>().RightZRotation) % 4];
            }


            if (Tiles[i].GetComponent<SocketData>().SymmetricEast)
            {
                Sockets[4, i] = Temp[4];
                Sockets[3, i + Tiles.Length] = Temp[4];
                Sockets[5, i + 2 * Tiles.Length] = Temp[4];
                Sockets[2, i + 3 * Tiles.Length] = Temp[4];
            }
            else
            {
                Sockets[4, i] = Temp[4] + RotationSocketOffset[Tiles[i].GetComponent<SocketData>().RightXRotation];
                Sockets[3, i + Tiles.Length] = Temp[4] + RotationSocketOffset[(1 + Tiles[i].GetComponent<SocketData>().RightXRotation) % 4];
                Sockets[5, i + 2 * Tiles.Length] = Temp[4] + RotationSocketOffset[(2 + Tiles[i].GetComponent<SocketData>().RightXRotation) % 4];
                Sockets[2, i + 3 * Tiles.Length] = Temp[4] + RotationSocketOffset[(3 + Tiles[i].GetComponent<SocketData>().RightXRotation) % 4];
            }
            if (Tiles[i].GetComponent<SocketData>().SymmetricSouth)
            {
                Sockets[3, i] = Temp[3];
                Sockets[5, i + Tiles.Length] = Temp[3];
                Sockets[2, i + 2 * Tiles.Length] = Temp[3];
                Sockets[4, i + 3 * Tiles.Length] = Temp[3];
            }
            else
            {
                Sockets[3, i] = Temp[3] + RotationSocketOffset[Tiles[i].GetComponent<SocketData>().LeftZRotation];
                Sockets[5, i + Tiles.Length] = Temp[3] + RotationSocketOffset[(1 + Tiles[i].GetComponent<SocketData>().LeftZRotation) % 4];
                Sockets[2, i + 2 * Tiles.Length] = Temp[3] + RotationSocketOffset[(2 + Tiles[i].GetComponent<SocketData>().LeftZRotation) % 4];
                Sockets[4, i + 3 * Tiles.Length] = Temp[3] + RotationSocketOffset[(3 + Tiles[i].GetComponent<SocketData>().LeftZRotation) % 4];
            }
            if (Tiles[i].GetComponent<SocketData>().SymmetricWest)
            {
                Sockets[5, i] = Temp[5];
                Sockets[2, i + Tiles.Length] = Temp[5];
                Sockets[4, i + 2 * Tiles.Length] = Temp[5];
                Sockets[3, i + 3 * Tiles.Length] = Temp[5];
            }
            else
            {
                Sockets[5, i] = Temp[5] + RotationSocketOffset[Tiles[i].GetComponent<SocketData>().LeftXRotation];
                Sockets[2, i + Tiles.Length] = Temp[5] + RotationSocketOffset[(1 + Tiles[i].GetComponent<SocketData>().LeftXRotation) % 4];
                Sockets[4, i + 2 * Tiles.Length] = Temp[5] + RotationSocketOffset[(2 + Tiles[i].GetComponent<SocketData>().LeftXRotation) % 4];
                Sockets[3, i + 3 * Tiles.Length] = Temp[5] + RotationSocketOffset[(3 + Tiles[i].GetComponent<SocketData>().LeftXRotation) % 4];
            }


        }
        Sockets[0, Tiles.Length * 4]=0;//air
        Sockets[1, Tiles.Length * 4]=0;
        Sockets[2, Tiles.Length * 4]=0;
        Sockets[3, Tiles.Length * 4]=0;
        Sockets[4, Tiles.Length * 4]=0;
        Sockets[5, Tiles.Length * 4]=0;
       
        Sockets[0, Tiles.Length * 4+1]=-1;//underground
        Sockets[1, Tiles.Length * 4+1]=-1;
        Sockets[2, Tiles.Length * 4+1]=-1;
        Sockets[3, Tiles.Length * 4+1]=-1;
        Sockets[4, Tiles.Length * 4+1]=-1;
        Sockets[5, Tiles.Length * 4+1]=-1;
        
        TileWeights[Tiles.Length * 4] = 1;
        TileWeights[Tiles.Length * 4+1] = 1;
    
        TileMap = new int[SizeX, SizeY, SizeZ];
        AvailableTiles = new int[SizeX, SizeY, SizeZ, TotalTiles];
        Entropy = new int[SizeX, SizeY, SizeZ];

        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                for (int z = 0; z < SizeZ; z++)
                {
                    TileMap[x, y, z] = Tiles.Length * 4;
                    Entropy[x, y, z] = TotalTiles;
                    for (int Tile = 0; Tile < TotalTiles; Tile++)
                    {
                        AvailableTiles[x, y, z, Tile] = 1;
                    }
                }
            }
        }

        CheckedTiles = new int[SizeX, SizeY, SizeZ];
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                for (int z = 0; z < SizeZ; z++)
                {
                    CheckedTiles[x, y, z] = 0;
                }
            }
        }

        PresetTiles();
        while (SetTiles(SizeX * SizeY * SizeZ - UsedTiles)) { /*Debug.Log("Here");*/ RemoveAllTiles(); UsedTiles=0; PresetTiles();}

        int[] p = new int[] { 3, 4, 0 };
        BFS_WalkTile(p[0], p[1], p[2], 1);
        p = BFS();

        while (CheckedTiles[p[0],p[1],p[2]]<25) 
        {
            //Debug.Log("Low WalkAblility");
            RemoveAllTiles(); 
            UsedTiles = 0;
            PresetTiles();
            while (SetTiles(SizeX * SizeY * SizeZ - UsedTiles)) {/* Debug.Log("Here");*/ RemoveAllTiles(); UsedTiles = 0; PresetTiles(); }

            p = new int[] { 3, 4, 0 };
            BFS_WalkTile(p[0], p[1], p[2], 1);
            p = BFS();
            
        }



         /* CheckedTiles = new int[SizeX,SizeY,SizeZ];
          for (int x = 0; x < SizeX; x++)
          {
              for (int y = 0; y < SizeY; y++)
              {
                  for (int z = 0; z < SizeZ; z++)
                  {
                      CheckedTiles[x, y, z] = 0;
                  }
              }
          }*/

          //int[] p = new int[] {3,4,0};
          //BFS_WalkTile(p[0], p[1], p[2], 1);
          //p = BFS();

          //Instantiate(Error, this.transform.position + new Vector3(p[0] * 20.0f, p[1] * 20.0f + 15.0f, p[2] * 20.0f), Quaternion.identity);
      

          /*Remove3x3(p[0], p[2]);
          CalculateEntropy(p[0], p[2]);
          SetTiles(3 * 3 * SizeY);*/



        //if (SetTiles(3 * 3 * SizeY)) { Debug.Log("Error"); }
        //while (SetTiles(3 * 3 * SizeY)) {
        //    Remove3x3(p[0], p[2]);
        //    CalculateEntropy(p[0], p[2]);
        //}


        /*while (CheckedTiles[SizeX - 4, 3, SizeZ - 1] == 0) 
        {
           Debug.Log(CheckedTiles[p[0], p[1], p[2]]);
           
          
           Remove3x3(p[0], p[2]);
           CalculateEntropy(p[0],p[2]);
           SetTiles(3 * 3 * SizeY);
           
           if (CheckedTiles[p[0] - 1, p[1], p[2]] > 0) { p[0]--; }
           else if (CheckedTiles[p[0] + 1, p[1], p[2]] > 0) { p[0]++; }
           else if (CheckedTiles[p[0], p[1], p[2] - 1] > 0) { p[2]--; }
           else if (CheckedTiles[p[0], p[1], p[2] + 1] > 0) { p[2]++; }

           BFS_WalkTile(p[0], p[1], p[2], 1);
           p = BFS();
        } */

        //Remove3x3(p[0], p[2]);


        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                for (int z = 0; z < SizeZ; z++)
                {

                    //if (CheckedTiles[x, y, z]!=0) {
                    //    Instantiate(Error, this.transform.position + new Vector3(x * 20.0f, y * 20.0f+15.0f+ CheckedTiles[x, y, z], z * 20.0f), Quaternion.identity); }
                    if (TileMap[x, y, z]< TotalTiles-2)
                    {
                        SocketData data;
                        if (TileMap[x, y, z] < Tiles.Length) {
                            data = Instantiate(Tiles[TileMap[x, y, z]], this.transform.position + new Vector3(x * 20.0f, y * 20.0f, z * 20.0f), Quaternion.identity).GetComponent<SocketData>();
                        }
                        else if (TileMap[x, y, z] < 2 * Tiles.Length) {
                            data = Instantiate(Tiles[TileMap[x, y, z] - Tiles.Length], this.transform.position + new Vector3(x * 20.0f, y * 20.0f, z * 20.0f), Quaternion.AngleAxis(90, new Vector3(0, 1, 0))).GetComponent<SocketData>();
                        }
                        else if (TileMap[x, y, z] < 3 * Tiles.Length) {
                            data = Instantiate(Tiles[TileMap[x, y, z] - 2 * Tiles.Length], this.transform.position + new Vector3(x * 20.0f, y * 20.0f, z * 20.0f), Quaternion.AngleAxis(180, new Vector3(0, 1, 0))).GetComponent<SocketData>();
                        }
                        else {
                            data = Instantiate(Tiles[TileMap[x, y, z] - 3 * Tiles.Length], this.transform.position + new Vector3(x * 20.0f, y * 20.0f, z * 20.0f), Quaternion.AngleAxis(270, new Vector3(0, 1, 0))).GetComponent<SocketData>();
                        }

                        data.WalkDistance = CheckedTiles[x, y, z];
                        //Optional
                        //data.UpAjacentType = Sockets[0, TileMap[x, y, z]];
                        //data.DownAjacentType = Sockets[1, TileMap[x, y, z]];
                        //data.RightZAjacentType = Sockets[2, TileMap[x, y, z]];
                        //data.LeftZAjacentType = Sockets[3, TileMap[x, y, z]];
                        //data.RightXAjacentType = Sockets[4, TileMap[x, y, z]];
                        //data.LeftXAjacentType = Sockets[5, TileMap[x, y, z]];
                    }
                }
            }
        }
        surface.BuildNavMesh();
    }
}
