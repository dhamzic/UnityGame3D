using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maze : MonoBehaviour
{
    public int width = 30; //x length
    public int depth = 30; //z length
    public byte[,] map; //matrix

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

    //Za kreiranje koridora labirinta, kreiranja puta kroz blokove
    void Generate()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Random.Range(0, 100) < 50){
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
                    Vector3 pos = new Vector3(x, 0, z);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.position = pos;
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
