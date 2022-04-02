using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    #region SCORE DISPLAY

    public int Score => score;
    public int Combo => combo;
    public float a;
    int score;
    int combo;
    int currentScore;

    Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);

    public void ResetScore()
    {
        score = 0;
        currentScore = 0;
        ScoreDisplay.UpdateText(score);
    }
    public void ResetCombo()
    {
        combo = 0;
        ComboText.UpdateText(score);
    }

    public void AddScore(int scorePoint)
    {
        currentScore += scorePoint;
        StartCoroutine(nameof(AddScoreCoroutine));
    }
    public void AddCombo(int comboPoint)
    {
        combo += comboPoint;
         a = combo / 50;
        //限制缩放大小(1-2)+
        a = Mathf.Clamp01(a);
       // Vector3 scale = new Vector3(1 + a, 1 + a, 1);
        ComboText.UpdateText(combo);
        ComboText.ScaleText(scoreTextScale);
    }
    public void EmptyCombo()
    {
        combo = 0;
        ComboText.UpdateText(combo);
        ComboText.ScaleText(Vector3.one);
    }
    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);

        while (score < currentScore)
        {
            score += 1;
            ScoreDisplay.UpdateText(score);
            yield return null;
        }

        ScoreDisplay.ScaleText(Vector3.one);
    }

    #endregion

    #region HIGH SCORE SYSTEM

    // [System.Serializable] public class PlayerScore
    // {
    //     public int score;
    //     public string playerName;

    //     public PlayerScore(int score, string playerName)
    //     {
    //         this.score = score;
    //         this.playerName = playerName;
    //     }
    // }

    // [System.Serializable] public class PlayerScoreData
    // {
    //     public List<PlayerScore> list = new List<PlayerScore>();
    // }

    // readonly string SaveFileName = "player_score.json";
    // string playerName = "No Name";

    // public bool HasNewHighScore => score > LoadPlayerScoreData().list[9].score;

    // public void SetPlayerName(string newName)
    // {
    //     playerName = newName;
    // }

    // public void SavePlayerScoreData()
    // {
    //     var playerScoreData = LoadPlayerScoreData();

    //     playerScoreData.list.Add(new PlayerScore(score, playerName));
    //     playerScoreData.list.Sort((x, y) => y.score.CompareTo(x.score));

    //     SaveSystem.Save(SaveFileName, playerScoreData);
    // }

    // public PlayerScoreData LoadPlayerScoreData()
    // {
    //     var playerScoreData = new PlayerScoreData();

    //     if (SaveSystem.SaveFileExists(SaveFileName))
    //     {
    //         playerScoreData = SaveSystem.Load<PlayerScoreData>(SaveFileName);
    //     }
    //     else
    //     {
    //         while (playerScoreData.list.Count < 10)
    //         {
    //             playerScoreData.list.Add(new PlayerScore(0, playerName));
    //         }

    //         SaveSystem.Save(SaveFileName, playerScoreData);
    //     }

    //     return playerScoreData;
    // }

    #endregion
}