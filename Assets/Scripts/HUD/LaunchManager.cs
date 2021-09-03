using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.HUD;
using Assets.Scripts.Memory;
using System;
using System.Linq;

public class LaunchManager : MonoBehaviour
{
    public List<Data> resultList = new List<Data>();
    public InputField playerName;
    private List<Room> listOfRooms = new List<Room>();
    private int currentListIndex = 0;

    Image roomImage;
    Text roomText;
    Text bestTimeText;

    GameObject leaderBoardPanel;


    void Start()
    {
        leaderBoardPanel = GameObject.Find("TablePanel");
        leaderBoardPanel.SetActive(false);

        //LoadResultsFromMemory();
        this.GetRooms();

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            this.playerName.text = PlayerPrefs.GetString("PlayerName");
        }
    }

    public void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }
    public void GetRooms()
    {
        Room room1 = new Room()
        {
            Name = "Caesar's room",
            Logo = Resources.Load("Room1_Image", typeof(Sprite)) as Sprite
        };
        Room room2 = new Room()
        {
            Name = "Morse's room",
            Logo = Resources.Load("Room2_Image", typeof(Sprite)) as Sprite
        };
        listOfRooms.Add(room1);
        listOfRooms.Add(room2);

        roomImage = GameObject.Find("Image").GetComponent<Image>();

        roomImage.sprite = this.listOfRooms[0].Logo;

        roomText = GameObject.Find("UiRoomText").GetComponent<Text>();
        roomText.text = "Room: " + this.listOfRooms[0].Name;
    }

    public void NextRoom()
    {
        if (this.listOfRooms.Count - 1 == this.currentListIndex)
        {
            this.currentListIndex = 0;
        }
        else
        {
            this.currentListIndex++;
        }
        roomImage.sprite = this.listOfRooms[currentListIndex].Logo;
        roomText.text = "Room: " + this.listOfRooms[currentListIndex].Name;
    }
    public void PreviousRoom()
    {
        if (this.currentListIndex == 0)
        {
            this.currentListIndex = this.listOfRooms.Count - 1;
        }
        else
        {
            this.currentListIndex--;
        }
        roomImage.sprite = this.listOfRooms[currentListIndex].Logo;
        roomText.text = "Room: " + this.listOfRooms[currentListIndex].Name;
    }

    public void ConnectNewScene()
    {
        SceneManager.LoadScene(this.listOfRooms[currentListIndex].Name);
    }

    public void LoadResultsFromMemory()
    {
        string resultsJson = PlayerPrefs.GetString("Result");
        if (!String.IsNullOrEmpty(resultsJson))
        {
            MemoryData md = JsonUtility.FromJson<MemoryData>(resultsJson);
            this.resultList = md.resultList;
        }
        leaderBoardPanel.SetActive(true);
        ShowResults(this.listOfRooms[currentListIndex].Name);
    }
    private void ShowResults(string roomName)
    {
        List<Data> resultsFromSpecficRoom = this.resultList
            .Where(n => n.RoomName == roomName)
            .OrderBy(t => t.Time)
            .Take(10)
            .ToList();

        Text positionText = GameObject.Find("UiPositionText").GetComponent<Text>();
        Text timeText = GameObject.Find("UiTimeText").GetComponent<Text>();
        Text nameText = GameObject.Find("UiNameText").GetComponent<Text>();

        roomText.text = "";
        positionText.text = "";
        timeText.text = "";
        nameText.text = "";

        int count = 1;
        foreach (Data result in resultsFromSpecficRoom)
        {
            TimeSpan time = TimeSpan.FromSeconds(result.Time);

            positionText.text = positionText.text + count + ". " + "\n";
            timeText.text = timeText.text + time.ToString(@"mm\:ss\:fff") + "\n";
            nameText.text = nameText.text + result.PlayerName + "\n";
            count++;
        }
    }

    public void EraseData()
    {
        PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            leaderBoardPanel.SetActive(false);
        }
    }
}
