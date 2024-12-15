using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreService : MonoBehaviour
{
    public static void SaveUserScoreList(string key, ScoreList userScoreList)
    {
        string json = JsonUtility.ToJson(userScoreList);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public static ScoreList LoadUserScoreList(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<ScoreList>(json);
        }

        return new ScoreList();
    }
}

[System.Serializable]
public class ScoreList
{
    public List<UserScore> scores;

    public ScoreList()
    {
        scores = new List<UserScore>();
    }
}

[System.Serializable]
public class UserScore
{
    public string user;
    public int points;

    public UserScore(string user, int points)
    {
        this.user = user;
        this.points = points;
    }
}