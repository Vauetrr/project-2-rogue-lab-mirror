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

    int[] RotationSocketOffset = { 0, 100000, 200000, 300000 };

    int[,] Sockets;
    int[,,] TileMap;
    int[,,] Entropy;
    int[,,,] AvailableTiles;
    int TotalTiles;
    void UpdateTile(int X, int Y, int Z, int I) 
    {
        for (int Index = 0; Index < TotalTiles; Index++)
        {
            if ((X + 1 < SizeX) && (AvailableTiles[X + 1, Y, Z, Index] != 0))
            {
                if (Sockets[4, I] == Sockets[5, Index])
                {
                    bool found = false;
                    for (int i =0;i< TotalTiles; i++) 
                    {
                        if ((AvailableTiles[X, Y, Z, i] != 0)&&(Sockets[4, i] == Sockets[5, Index]))
                        {
                            found = true; break;
                        }
                    }
                    if (found == false) 
                    {
                        AvailableTiles[X + 1, Y, Z, Index] = 0;
                        Entropy[X + 1, Y, Z]--;
                        UpdateTile(X + 1,Y,Z,Index); ;
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
                        AvailableTiles[X, Y+1, Z, Index] = 0;
                        Entropy[X, Y+1, Z]--;
                        UpdateTile(X, Y+1, Z, Index);
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
                        AvailableTiles[X, Y-1, Z, Index] = 0;
                        Entropy[X, Y-1, Z]--;
                        UpdateTile(X, Y-1, Z, Index);
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
                        AvailableTiles[X, Y, Z+1, Index] = 0;
                        Entropy[X, Y, Z+1]--;
                        UpdateTile(X, Y, Z+1, Index);
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
                        AvailableTiles[X, Y, Z-1, Index] = 0;
                        Entropy[X, Y, Z-1]--;
                        UpdateTile(X, Y, Z-1, Index); 
                    }
                }
            }
        }
    }

    


    // Start is called before the first frame update
    void Start()
    {
        bool Error0 = false;

        //int[,] Sockets = new int[6, Tiles.Length * 4];
        TotalTiles = (Tiles.Length * 4) + 2;
        Sockets = new int[6, TotalTiles];
        
        for (int i = 0; i < Tiles.Length; i++)
        {
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
                Sockets[0, i + Tiles.Length] = Temp[0];
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
        //int[,,] TileMap = new int[SizeX, SizeY, SizeZ];
        TileMap = new int[SizeX, SizeY, SizeZ];

        //int[,,,] AvailableTiles = new int[SizeX, SizeY, SizeZ, 4 * Tiles.Length];
        AvailableTiles = new int[SizeX, SizeY, SizeZ, TotalTiles];
        Entropy = new int[SizeX, SizeY, SizeZ];
        //int[,,] Entropy = new int[SizeX, SizeY, SizeZ];

        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                for (int z = 0; z < SizeZ; z++)
                {
                    Entropy[x, y, z] = TotalTiles;
                    for (int Tile = 0; Tile < TotalTiles; Tile++)
                    {
                        AvailableTiles[x, y, z, Tile] = 1;
                    }
                }
            }
        }
        int StartI = 1, StartX = 0, StartY = 4, StartZ = 0;
        { //StartLocation
            TileMap[StartX, StartY, StartZ] = StartI;
            Entropy[StartX, StartY, StartZ] = 1000000000;
            //int StartI = 1, StartX = SizeX/2, StartY = SizeY/2, StartZ = SizeZ/2;
            for (int Index = 0; Index < TotalTiles; Index++)
            {

                if ((StartX + 1 < SizeX) && (AvailableTiles[StartX + 1, StartY, StartZ, Index] != 0))
                {
                    if (Sockets[4, StartI] != Sockets[5, Index])
                    {
                        AvailableTiles[StartX + 1, StartY, StartZ, Index] = 0;
                        Entropy[StartX + 1, StartY, StartZ]--; //Debug.Log("X+1");
                    }
                }
                if ((StartX > 0) && (AvailableTiles[StartX - 1, StartY, StartZ, Index] != 0))
                {
                    if (Sockets[5, StartI] != Sockets[4, Index])
                    {
                        AvailableTiles[StartX - 1, StartY, StartZ, Index] = 0;
                        Entropy[StartX - 1, StartY, StartZ]--;
                    }
                }
                if ((StartY + 1 < SizeY) && (AvailableTiles[StartX, StartY + 1, StartZ, Index] != 0))
                {
                    if (Sockets[0, StartI] != Sockets[1, Index])
                    {
                        AvailableTiles[StartX, StartY + 1, StartZ, Index] = 0;
                        Entropy[StartX, StartY + 1, StartZ]--;
                    }
                }
                if ((StartY > 0) && (AvailableTiles[StartX, StartY - 1, StartZ, Index] != 0))
                {
                    if (Sockets[1, StartI] != Sockets[0, Index])
                    {
                        AvailableTiles[StartX, StartY - 1, StartZ, Index] = 0;
                        Entropy[StartX, StartY - 1, StartZ]--;
                    }
                }
                if ((StartZ + 1 < SizeZ) && (AvailableTiles[StartX, StartY, StartZ + 1, Index] != 0))
                {
                    if (Sockets[2, StartI] != Sockets[3, Index])
                    {
                        AvailableTiles[StartX, StartY, StartZ + 1, Index] = 0;
                        Entropy[StartX, StartY, StartZ + 1]--;// Debug.Log(Entropy[StartX, StartY, StartZ + 1]);
                    }
                }
                if ((StartZ > 0) && (AvailableTiles[StartX, StartY, StartZ - 1, Index] != 0))
                {
                    if (Sockets[3, StartI] != Sockets[2, Index])
                    {
                        AvailableTiles[StartX, StartY, StartZ - 1, Index] = 0;
                        Entropy[StartX, StartY, StartZ - 1]--;
                    }
                }
                AvailableTiles[StartX, StartY, StartZ, Index] = 0;
            }
            //Debug.Log("Here");
            //Debug.Log(Entropy[0, 5,  0]);
        }
        for (int TileIndex = 0; TileIndex < SizeX * SizeY * SizeZ - 1; TileIndex++)
        //for (int TileIndex = 0; TileIndex < 20; TileIndex++)
        {
            int lowest = 100000000;
            int X = 0, Y = 0, Z = 0;
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

                            if ((NewDist < Dist)) { X = x; Y = y; Z = z; lowest = Entropy[x, y, z]; }
                        }
                    }
                }
            }

            //int[] TryTile = new int[Entropy[X, Y, Z]];//new int[4 * Tiles.Length];

            int[] TryTile = new int[TotalTiles];
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

            if (index == 0 & Error0 == false) { Error0 = true; Debug.LogError("Unable To Find Tile (" + X + ", " + Y + ", " + Z + " ) Entropy[x,y,z] = " + Entropy[X, Y, Z]); Instantiate(Error, this.transform.position + new Vector3(X * 20.0f, Y * 20.0f, Z * 20.0f), Quaternion.identity); }
            TileMap[X, Y, Z] = TryTile[Random.Range(0, index)];
            Entropy[X, Y, Z] = 1000000000;
            int I = TileMap[X, Y, Z];



            for (int Index = 0; Index < TotalTiles; Index++)
            {
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
                        UpdateTile(X-1, Y, Z, Index);
                    }
                }
                if ((Y + 1 < SizeY) && (AvailableTiles[X, Y + 1, Z, Index] != 0))
                {
                    if (Sockets[0, I] != Sockets[1, Index])
                    {
                        AvailableTiles[X, Y + 1, Z, Index] = 0;
                        Entropy[X, Y + 1, Z]--;
                        UpdateTile(X, Y+1, Z, Index);
                    }
                }
                if ((Y > 0) && (AvailableTiles[X, Y - 1, Z, Index] != 0))
                {
                    if (Sockets[1, I] != Sockets[0, Index])
                    {
                        AvailableTiles[X, Y - 1, Z, Index] = 0;
                        Entropy[X, Y - 1, Z]--;
                        UpdateTile(X, Y-1, Z, Index);
                    }
                }
                if ((Z + 1 < SizeZ) && (AvailableTiles[X, Y, Z + 1, Index] != 0))
                {
                    if (Sockets[2, I] != Sockets[3, Index])
                    {
                        AvailableTiles[X, Y, Z + 1, Index] = 0;
                        Entropy[X, Y, Z + 1]--;
                        UpdateTile(X, Y, Z+1, Index);
                    }
                }
                if ((Z > 0) && (AvailableTiles[X, Y, Z - 1, Index] != 0))
                {
                    if (Sockets[3, I] != Sockets[2, Index])
                    {
                        AvailableTiles[X, Y, Z - 1, Index] = 0;
                        Entropy[X, Y, Z - 1]--;
                        UpdateTile(X, Y, Z-1, Index);
                    }
                }
            }
        }


        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                for (int z = 0; z < SizeZ; z++)
                {

                    if (TileMap[x, y, z]< TotalTiles-2)
                    {
                        SocketData data;
                        if (TileMap[x, y, z] < Tiles.Length)
                        {
                            data = Instantiate(Tiles[TileMap[x, y, z]], this.transform.position + new Vector3(x * 20.0f, y * 20.0f, z * 20.0f), Quaternion.identity).GetComponent<SocketData>();
                        }
                        else if (TileMap[x, y, z] < 2 * Tiles.Length)
                        {
                            data = Instantiate(Tiles[TileMap[x, y, z] - Tiles.Length], this.transform.position + new Vector3(x * 20.0f, y * 20.0f, z * 20.0f), Quaternion.AngleAxis(90, new Vector3(0, 1, 0))).GetComponent<SocketData>();
                            //Instantiate(Tiles[TileMap[x, y, z]-Tiles.Length], this.transform.position + new Vector3(x * 20.0f, y * 20.0f, z * 20.0f), Quaternion.identity );//Quaternion.AxisAngle(new Vector3(0,1,0),-Mathf.PI/2));
                        }
                        else if (TileMap[x, y, z] < 3 * Tiles.Length)
                        {
                            data = Instantiate(Tiles[TileMap[x, y, z] - 2 * Tiles.Length], this.transform.position + new Vector3(x * 20.0f, y * 20.0f, z * 20.0f), Quaternion.AngleAxis(180, new Vector3(0, 1, 0))).GetComponent<SocketData>();
                            //Instantiate(Tiles[TileMap[x, y, z] - 2*Tiles.Length], this.transform.position + new Vector3(x * 20.0f, y * 20.0f, z * 20.0f), Quaternion.identity);
                        }
                        else
                        {
                            data = Instantiate(Tiles[TileMap[x, y, z] - 3 * Tiles.Length], this.transform.position + new Vector3(x * 20.0f, y * 20.0f, z * 20.0f), Quaternion.AngleAxis(270, new Vector3(0, 1, 0))).GetComponent<SocketData>();
                            //Instantiate(Tiles[TileMap[x, y, z] - 3*Tiles.Length], this.transform.position + new Vector3(x * 20.0f, y * 20.0f, z * 20.0f), Quaternion.identity);
                        }

                        data.UpAjacentType = Sockets[0, TileMap[x, y, z]];
                        data.DownAjacentType = Sockets[1, TileMap[x, y, z]];
                        data.RightZAjacentType = Sockets[2, TileMap[x, y, z]];
                        data.LeftZAjacentType = Sockets[3, TileMap[x, y, z]];
                        data.RightXAjacentType = Sockets[4, TileMap[x, y, z]];
                        data.LeftXAjacentType = Sockets[5, TileMap[x, y, z]];
                    }
                }
            }
        }
        surface.BuildNavMesh();
    }
}
