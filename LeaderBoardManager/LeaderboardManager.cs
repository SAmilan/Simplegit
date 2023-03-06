using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class LeaderBoardData
{
    public string playerName;
    public int playerScore;
    public int playerRank;
   public LeaderBoardData (string name, int score, int rank)
    {
        playerName = name;
        playerRank = rank;
        playerScore = score;
    }
}

public class LeaderBoardDataList
{
   public List<LeaderBoardData> leaderboardlists;
}

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;
    string filedestination = null;

    private void Awake()
    {
        instance = this;
    }

    public void SaveFile(List<LeaderBoardData> leaderBoardData)
    {
        LeaderBoardDataList boardData = new LeaderBoardDataList();
        boardData.leaderboardlists = LoadFile();
        foreach(LeaderBoardData dataObj in leaderBoardData)
        {
            boardData.leaderboardlists.Add(dataObj);
        }
        string jsonString = JsonUtility.ToJson(boardData);
        PlayerPrefs.SetString("LEADER_BOARD", jsonString);
        PlayerPrefs.Save();
        Debug.Log("File sucessfully saved");
    }

    public List<LeaderBoardData> LoadFile()
    {
        List<LeaderBoardData> data = new List<LeaderBoardData>();
        if (PlayerPrefs.GetString("LEADER_BOARD") != "")
        {
            Debug.Log(PlayerPrefs.GetString("LEADER_BOARD"));
            string jsonString = PlayerPrefs.GetString("LEADER_BOARD"); ;
            LeaderBoardDataList boardData = new LeaderBoardDataList();
            boardData = JsonUtility.FromJson<LeaderBoardDataList>(jsonString);
            data = boardData.leaderboardlists;
            Debug.Log("data loaded");
        }
        return data;
    }
}
