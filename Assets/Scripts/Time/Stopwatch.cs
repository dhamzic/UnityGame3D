using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Stopwatch : MonoBehaviour
{
    bool timerActive = false;
    public float currentTime;
    public Text currentTimeText;

    void Start()
    {
        //Postavljanje trenutno vremena na 0
        currentTime = 0;
        //Pokretanje štoperice
        StartStopwatch();
    }
    void Update()
    {
        if (timerActive == true)
        {
            //-Time.deltaTime jer 60fps nekad zna biti varijabilan zbog rada CPU-a
            currentTime = currentTime + Time.deltaTime;
        }
        //Pretvorba float tipa podatka u oblik vremena razumljiv igraču
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss\:fff");
    }
    //Početak igre
    public void StartStopwatch()
    {
        timerActive = true;
    }
    //Zaustavljanje štoperice (Gotova Igra)
    public void StopStopwatch()
    {
        timerActive = false;
    }
}
