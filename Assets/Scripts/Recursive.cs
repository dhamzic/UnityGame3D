using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recursive : Maze
{
    public override void Generate()
    {
        //Prvo treba pronači početno točku tj. početni blok
        //WIDTH == širina labirinta tj. broj kocka po horizontali, ne smije biti ni 1 ni count(width)
        //jer bi mogao isrctati prazan prostor na granici labirinta
        Generate(Random.Range(1, width), Random.Range(1, depth));
    }
    void Generate(int x, int z)
    {
        if (CountSquareNeighbours(x, z) >= 2)
        {
            return;
        }
        //Kreiramo prazan blok tj. prostor
        map[x, z] = 0;

        //Promiješaj smjerove kreiranja tunela kroz labirint
        directions.Shuffle();

        Generate(x + directions[0].x, z + directions[0].z);
        Generate(x + directions[1].x, z + directions[1].z);
        Generate(x + directions[2].x, z + directions[2].z);
        Generate(x + directions[3].x, z + directions[3].z);
    }
}
