using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Stopwatch : MonoBehaviour
{
    bool timerActive = false;
    float currentTime;
    public Text currentTimeText;

    // Start is called before the first frame update
    void Start()
    {
        //Sekunde
        currentTime = 0;
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive == true)
        {
            //-Time.deltaTime jer 60fps nekad zna biti varijabilan zbog rada CPU-a
            currentTime = currentTime + Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss\:fff");
    }
    public void StartTimer()
    {
        timerActive = true;
    }
    public void StopTimer()
    {
        timerActive = false;
    }
}
