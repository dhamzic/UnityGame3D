using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.HUD;

public class LaunchManager : MonoBehaviour
{
    public InputField playerName;
    private List<Room> listOfRooms = new List<Room>();
    private int currentListIndex = 0;

    Image roomImage;
    Text roomText;
    Text bestTimeText;


    void Start()
    {
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
            Logo = Resources.Load("Room1_Image", typeof(Sprite)) as Sprite,
            BestTime = 2.23f
        };
        Room room2 = new Room()
        {
            Name = "NoName room",
            Logo = Resources.Load("Room2_Image", typeof(Sprite)) as Sprite,
            BestTime = 5.32f
        };
        listOfRooms.Add(room1);
        listOfRooms.Add(room2);

        roomImage = GameObject.Find("Image").GetComponent<Image>();

        roomImage.sprite = this.listOfRooms[0].Logo;

        roomText = GameObject.Find("UiRoomText").GetComponent<Text>();
        roomText.text = "Room: " + this.listOfRooms[0].Name;

        bestTimeText = GameObject.Find("UiBestTimeText").GetComponent<Text>();
        bestTimeText.text = "Best time: " + this.listOfRooms[0].BestTime.ToString();
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
        bestTimeText.text = "Best time: " + this.listOfRooms[currentListIndex].BestTime.ToString();
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
        bestTimeText.text = "Best time: " + this.listOfRooms[currentListIndex].BestTime.ToString();
    }

    public void ConnectNewScene()
    {
        SceneManager.LoadScene("Room1");
    }


    // Update is called once per frame
    void Update()
    {

    }
}
