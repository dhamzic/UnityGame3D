using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    bool timerActive = false;
    float currentTime;
    public int startMinutes;
    public Text currentTimeText;



    // Start is called before the first frame update
    void Start()
    {
        //Sekunde
        currentTime = startMinutes * 60;
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive == true)
        {
            //-Time.deltaTime jer 60fps nekad zna biti varijabilan zbog rada CPU-a
            currentTime = currentTime - Time.deltaTime;
            if (currentTime <= 0) {
                StopTimer();
                EndGame endGameScript = GameObject.Find("FloorExit").GetComponent<EndGame>();
                endGameScript.GameOver("Time's Up!");
            }
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
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
