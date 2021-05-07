using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wilsons : Maze
{
    

    //Koristi se za evidenciju blokova koji služe za početak novog puta labirinta. Svaki element liste
    //prestavlja element koji nema susjeda koji cini vec postojeci labirint. Ovo rijesava problem
    //kada novo kreirani put pocinje u bloku koji vec cini prethodni put pa dolazi do promjene u sirini
    //puta (prazan blok jedan pored drugog)
    List<MapLocation> notUsed = new List<MapLocation>();
    public override void Generate()
    {
        //1. Odabir random kocke/čelije tj. prazna kocka/čelija
        int x = Random.Range(2, width - 1);
        int z = Random.Range(2, depth - 1);
        map[x, z] = 2;

        //Ukoliko postoji slobodan blok za novi početak puta
        while (GetAvailableCells() > 1)
        {
            RandomWalk();
        }


    }

    int CountSquareMazeNeighbours(int x, int z)
    {
        int count = 0;
        for (int d = 0; d < directions.Count; d++)
        {
            int nextX = x + directions[d].x;
            int nextZ = z + directions[d].z;

            if (map[nextX, nextZ] == 2)
            {
                count++;
            }
        }
        return count;
    }

    //Pronalazi potencijalni novi start za novi put labirinta
    int GetAvailableCells()
    {
        notUsed.Clear();
        //Pronađi blokove koji nemaju susjeda u labirintu tj. susjede koji nisu prazni blokovi
        for (int z = 1; z < depth - 1; z++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                if (CountSquareMazeNeighbours(x, z) == 0)
                {
                    notUsed.Add(new MapLocation(x, z));
                }
            }
        }
        return notUsed.Count;
    }

    //Šetnja od random početnog praznog bloka do granice
    void RandomWalk()
    {
        List<MapLocation> inWalk = new List<MapLocation>();

        //Početna pozicija
        int currentX;
        int currentZ;
        int randomStartIndex = Random.Range(0, notUsed.Count);

        currentX = notUsed[randomStartIndex].x;
        currentZ = notUsed[randomStartIndex].z;

        //Provjeri je li novi put ima ispravan broj susjeda
        inWalk.Add(new MapLocation(currentX, currentZ));

        //TODO: Izbrisi, testiranje
        int loop = 0;
        bool validPath = false;

        //Loop sve dok currentX/currentZ ne dođe do granice mape
        while (currentX > 0 && currentX < width - 1 && currentZ > 0 && currentZ < depth - 1 && loop < 5000 && !validPath)
        {
            map[currentX, currentZ] = 0;

            //Sprjećava dodatne blokove tj. prazan blok pored praznog tj. šire blokove.
            if (CountSquareMazeNeighbours(currentX, currentZ) > 1) {
                break;
            }

            int randomDirection = Random.Range(0, directions.Count);

            //Mogući sljedeći koridor (prazna kocka)
            int nextX = currentX + directions[randomDirection].x;
            int nextZ = currentZ + directions[randomDirection].z;

            //Ako puna kocka ima manje od 2 susjeda, kreiraj novu praznu kocku (koridor)
            if (CountSquareNeighbours(nextX, nextZ) < 2)
            {
                currentX = nextX;
                currentZ = nextZ;
                inWalk.Add(new MapLocation(currentX, currentZ));
            }
            validPath = CountSquareMazeNeighbours(currentX, currentZ) == 1;


            loop++;
        }

        if (validPath)
        {
            map[currentX, currentZ] = 0;
            Debug.Log("PathFound");

            foreach (MapLocation ml in inWalk)
            {
                //Postavi prolaz tj. prazan blok
                map[ml.x, ml.z] = 2;
            }
            inWalk.Clear();
        }
        else
        {
            foreach (MapLocation ml in inWalk)
            {
                //Postavi ih 1 što znaci da je to zid tj. puna kocka
                map[ml.x, ml.z] = 1;
            }
            inWalk.Clear();
        }
    }
}
