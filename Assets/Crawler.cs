using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : Maze
{
    public override void Generate()
    {
        for (int i = 0; i < 2; i++)
        {
            CrawlVertical();
        }
        CrawlVertical();
        for (int i = 0; i < 3; i++)
        {
            CrawlHorizontal();
        }

    }

    void CrawlVertical()
    {
        bool done = false;
        //Početna pozicija crawler-a u sredini
        int x = Random.Range(1, width - 1);
        int z = 1;

        while (!done)
        {
            map[x, z] = 0;
            //Random generira -1, 0, 1
            if (Random.Range(0, 100) < 50)
                x += Random.Range(-1, 2);
            else
                z += Random.Range(0, 2);
            //Zaustavi se kad dođeš do kraja labirinta
            if (x < 1 || x >= width - 1 || z < 1 || z >= depth - 1)
            {
                done = true;
            }
            else
            {
                done = false;
            }
        }
    }
    void CrawlHorizontal()
    {
        bool done = false;
        //Početna pozicija crawler-a u sredini
        int x = 1;
        int z = Random.Range(1, depth - 1);

        while (!done)
        {
            map[x, z] = 0;
            //Random generira -1, 0, 1
            if (Random.Range(0, 100) < 50)
                x += Random.Range(0, 2);
            else
                z += Random.Range(-1, 2);
            //Zaustavi se kad dođeš do kraja labirinta
            if (x < 1 || x >= width - 1 || z < 1 || z >= depth - 1)
            {
                done = true;
            }
            else
            {
                done = false;
            }
        }
    }

}
