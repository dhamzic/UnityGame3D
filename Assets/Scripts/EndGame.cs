using Assets.Scripts.Memory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {

        Stopwatch bsa = GameObject.Find("UiStopwatch").GetComponent<Stopwatch>();
        bsa.StopTimer();
        StoreData(bsa.currentTime);
    }
    public void StoreData(float time)
    {
        PlayerPrefs.DeleteKey("Result");
        MemoryData md = new MemoryData();

        string roomName = SceneManager.GetActiveScene().name;
        string playerName = PlayerPrefs.GetString("PlayerName");

        Data result = new Data();
        result.PlayerName = playerName;
        result.Time = time;
        result.RoomName = roomName;

        md.resultList.Add(result);
        md.resultList.Add(result);

        string resultsJson = JsonUtility.ToJson(md);

        PlayerPrefs.SetString("Result", resultsJson);
        PlayerPrefs.Save();

        string results = PlayerPrefs.GetString("Result");

    }
}
