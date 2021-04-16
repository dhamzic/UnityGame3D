using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation
{
    public int x;
    public int z;

    public MapLocation(int xLoc, int zLoc)
    {
        this.x = xLoc;
        this.z = zLoc;
    }
}
public class Maze : MonoBehaviour
{
    //Za kretnje, lijevo, desno, gore, dolje
    public List<MapLocation> directions = new List<MapLocation>() {
        new MapLocation(1,0), //desno
        new MapLocation(0,1), //gore
        new MapLocation(-1,0), //lijevo
        new MapLocation(0,-1) //dolje
    };

    public int width = 30; //x length
    public int depth = 30; //z length
    public byte[,] map; //matrix
    public int scale = 6;

    // Start is called before the first frame update
    void Start()
    {
        InitialiseMap();
        Generate();
        DrawMap();
    }

    //Kreiranje svih blokova
    void InitialiseMap()
    {
        map = new byte[width, depth];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1; //1 = wall
            }
        }
    }

    //Virtual -> ova metoda može biti overridana-a u klasi koja nasljeđuje ovu
    //Za kreiranje koridora labirinta, kreiranja puta kroz blokove
    public virtual void Generate()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Random.Range(0, 100) < 50)
                {
                    map[x, z] = 0; //1 = corridor
                }
            }
        }
    }

    void DrawMap()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                //Ako je zid nacrtaj ga
                if (map[x, z] == 1)
                {
                    //x i z su pozicije objekta
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;
                }

            }
        }
    }

    //Susjed == prazna kocka, "rupa"
    //Vraća broj blokova koji predstavljaju koridore tj. kocke koje se ne iscrtavaju i predstavljaju put
    //map[x, z] == 0?
    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;

        //Rub mape, to nas ne zanima
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;

        return count;
    }

    //Provjera slobodnog puta po dijagonalama
    public int CountDiagonalNeighbours(int x, int z)
    {
        int count = 0;

        //Rub mape, to nas ne zanima
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z - 1] == 0) count++;
        if (map[x + 1, z + 1] == 0) count++;
        if (map[x - 1, z + 1] == 0) count++;
        if (map[x + 1, z - 1] == 0) count++;

        return count;
    }

    public int CountAllNeighbours(int x, int z)
    {
        return CountSquareNeighbours(x, z) + CountDiagonalNeighbours(x, z);
    }
}
