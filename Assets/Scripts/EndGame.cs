using Assets.Scripts.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    GameObject mainCamera;
    GameObject player;
    GameObject endGamePanel;

    public AudioSource endGameSound;

    private GameObject UiInventoryCanvas;
    private GameObject UiInventoryRead;
    private GameObject RemoteControlCanvas;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        player = GameObject.Find("FPC");
        endGamePanel = GameObject.Find("UiEndGamePanel");
        endGamePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        GameOver("You Escaped!");
    }
    public void GameOver(string title)
    {
        UiInventoryCanvas = GameObject.Find("UiInventory");
        if (UiInventoryCanvas != null)
        {
            UiInventoryCanvas.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "Morse's room")
        {
            RemoteControlCanvas = GameObject.Find("UiRemoteControllerCanvas");
            if (RemoteControlCanvas != null)
            {
                RemoteControlCanvas.SetActive(false);
            }
        }
        if (SceneManager.GetActiveScene().name == "Caesar's room")
        {
            UiInventoryRead = GameObject.Find("UiInventoryRead");
            if (UiInventoryRead != null)
            {
                UiInventoryRead.SetActive(false);

            }
        }



        endGameSound.Play();

        FPController D = player.GetComponent<FPController>();
        D.gameEnded = true;
        D.enabled = false;

        PlayerControls pc = mainCamera.GetComponent<PlayerControls>();
        pc.enabled = false;

        Stopwatch bsa = GameObject.Find("UiStopwatch").GetComponent<Stopwatch>();
        bsa.StopStopwatch();

        StoreData(bsa.currentTime);

        endGamePanel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Text textTitle = GameObject.Find("Text").GetComponent<Text>();
        textTitle.text = title;
        Text timeText = GameObject.Find("UiTimeTxt").GetComponent<Text>();
        //Text rankText = GameObject.Find("UiRankTxt").GetComponent<Text>();

        TimeSpan time = TimeSpan.FromSeconds(bsa.currentTime);
        timeText.text = "Time: " + time.ToString(@"mm\:ss\:fff");

        GetRanks(bsa.currentTime);
    }

    public void GetRanks(float currentTime)
    {
        float r = 0f, g = 0.4168899f, b = 0.9716981f, a = 1f;
        List<Data> resultList = new List<Data>();
        List<Data> resultListTopFive = new List<Data>();
        string resultsJson = PlayerPrefs.GetString("Result");
        if (!String.IsNullOrEmpty(resultsJson))
        {
            MemoryData md = JsonUtility.FromJson<MemoryData>(resultsJson);
            resultList = md.resultList;
            resultList = resultList.OrderBy(t => t.Time).ToList();
        }
        resultListTopFive = resultList.Where(n => n.RoomName == SceneManager.GetActiveScene().name).Take(5).ToList();

        Text position = GameObject.Find("Sixth").GetComponent<Text>();
        Text name = GameObject.Find("NameSixth").GetComponent<Text>();
        Text timeText = GameObject.Find("SixthTime").GetComponent<Text>();

        int yourPositionIndex = resultList.FindIndex(t => t.Time == currentTime && t.PlayerName == PlayerPrefs.GetString("PlayerName"));



        //Trenutni rezultat nije u Top5
        if (resultListTopFive.Where(t => t.Time == currentTime && t.PlayerName == PlayerPrefs.GetString("PlayerName")).FirstOrDefault() == null)
        {

            position.color = new Color(r, g, b, a);
            name.color = new Color(r, g, b, a);
            timeText.color = new Color(r, g, b, a);

            position.text = yourPositionIndex + 1 + ".";
            name.text = PlayerPrefs.GetString("PlayerName");
            timeText.text = TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss\:fff");
        }
        else
        {
            //U top 5 je trenutni rezultat. Obriši dodatni red
            position.text = "";
            name.text = "";
            timeText.text = "";
        }




        position = GameObject.Find("First").GetComponent<Text>();
        name = GameObject.Find("NameFirst").GetComponent<Text>();
        timeText = GameObject.Find("FirstTime").GetComponent<Text>();
        if (resultListTopFive[0] != null)
        {
            position.text = "1.";
            name.text = resultListTopFive[0].PlayerName;
            timeText.text = TimeSpan.FromSeconds(resultListTopFive[0].Time).ToString(@"mm\:ss\:fff");
            if (yourPositionIndex == 0)
            {
                position.color = new Color(r, g, b, a);
                name.color = new Color(r, g, b, a);
                timeText.color = new Color(r, g, b, a);
            }
        }
        else
        {
            position.text = "";
            name.text = "";
            timeText.text = "";
        }
        position = GameObject.Find("Second").GetComponent<Text>();
        name = GameObject.Find("NameSecond").GetComponent<Text>();
        timeText = GameObject.Find("SecondTime").GetComponent<Text>();
        if (resultListTopFive.Count > 1)
        {
            position.text = "2.";
            name.text = resultListTopFive[1].PlayerName;
            timeText.text = TimeSpan.FromSeconds(resultListTopFive[1].Time).ToString(@"mm\:ss\:fff");
            if (yourPositionIndex == 1)
            {
                position.color = new Color(r, g, b, a);
                name.color = new Color(r, g, b, a);
                timeText.color = new Color(r, g, b, a);

            }
        }
        else
        {
            position.text = "";
            name.text = "";
            timeText.text = "";
        }
        position = GameObject.Find("Third").GetComponent<Text>();
        name = GameObject.Find("NameThird").GetComponent<Text>();
        timeText = GameObject.Find("ThirdTime").GetComponent<Text>();
        if (resultListTopFive.Count > 2)
        {
            position.text = "3.";
            name.text = resultListTopFive[2].PlayerName;
            timeText.text = TimeSpan.FromSeconds(resultListTopFive[2].Time).ToString(@"mm\:ss\:fff");
            if (yourPositionIndex == 2)
            {
                position.color = new Color(r, g, b, a);
                name.color = new Color(r, g, b, a);
                timeText.color = new Color(r, g, b, a);

            }
        }
        else
        {
            position.text = "";
            name.text = "";
            timeText.text = "";
        }
        position = GameObject.Find("Fourth").GetComponent<Text>();
        name = GameObject.Find("NameFourth").GetComponent<Text>();
        timeText = GameObject.Find("FourthTime").GetComponent<Text>();
        if (resultListTopFive.Count > 3)
        {
            position.text = "4.";
            name.text = resultListTopFive[3].PlayerName;
            timeText.text = TimeSpan.FromSeconds(resultListTopFive[3].Time).ToString(@"mm\:ss\:fff");
            if (yourPositionIndex == 3)
            {
                position.color = new Color(r, g, b, a);
                name.color = new Color(r, g, b, a);
                timeText.color = new Color(r, g, b, a);
            }
        }
        else
        {
            position.text = "";
            name.text = "";
            timeText.text = "";
        }
        position = GameObject.Find("Fifth").GetComponent<Text>();
        name = GameObject.Find("NameFifth").GetComponent<Text>();
        timeText = GameObject.Find("FifthTime").GetComponent<Text>();
        if (resultListTopFive.Count > 4)
        {
            position.text = "5.";
            name.text = resultListTopFive[4].PlayerName;
            timeText.text = TimeSpan.FromSeconds(resultListTopFive[4].Time).ToString(@"mm\:ss\:fff");
            if (yourPositionIndex == 4)
            {
                position.color = new Color(r, g, b, a);
                name.color = new Color(r, g, b, a);
                timeText.color = new Color(r, g, b, a);
            }
        }
        else
        {
            position.text = "";
            name.text = "";
            timeText.text = "";
        }

    }

    public void StoreData(float time)
    {
        string loadedResultsJson = PlayerPrefs.GetString("Result");
        MemoryData loadedMemory = new MemoryData();
        if (!String.IsNullOrEmpty(loadedResultsJson))
        {
            loadedMemory = JsonUtility.FromJson<MemoryData>(loadedResultsJson);
        }

        string roomName = SceneManager.GetActiveScene().name;
        string playerName = PlayerPrefs.GetString("PlayerName");

        Data result = new Data();
        result.PlayerName = playerName;
        result.Time = time;
        result.RoomName = roomName;

        loadedMemory.resultList.Add(result);

        string resultsJson = JsonUtility.ToJson(loadedMemory);

        PlayerPrefs.SetString("Result", resultsJson);
        PlayerPrefs.Save();
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
