using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour
{
    public GameManager2 gameManager;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TextMeshProUGUI[] nameText, scoreText;
    private List<String> nameList = new List<string>();
    private List<int> scoreList = new List<int>();

    public string[] names;
    public int[] scores;

    private void Start() {
        LoadGame();
    }
  
    public void SaveToLeaderBoard() {
        if (string.IsNullOrWhiteSpace(nameInput.text))
        {
            Debug.Log("game not saved");
        }else {
            CheckForScore();
            SaveGame();
            LoadGame();
        }
    }

    private void CheckForScore() {
        scoreList.Add(gameManager.totalScore);
        scoreList.Sort();
        scoreList.Reverse();
        nameList.Insert(scoreList.IndexOf(gameManager.totalScore), nameInput.text);
        if (scoreList.Count > scoreText.Length)
        {
            nameList.RemoveAt(nameList.Count - 1);
            scoreList.RemoveAt(scoreList.Count - 1);
        }
    }

    private void SaveGame() {
        names = new string[nameList.Count];
        scores = new int[scoreList.Count];
        for (int i = 0; i < nameList.Count; i++)
        {
            names[i] = nameList[i];
            scores[i] = scoreList[i];
        }
        nameList.Clear();
        scoreList.Clear();
        LeaderboardSystem.SaveGame(this);
    }

    private void LoadGame() {
        LeaderboardModel saveData = LeaderboardSystem.LoadGame();
        if (saveData != null)
        {
            names = saveData.names;
            scores = saveData.scores;
        }   
        foreach (var c in names)
        {
            nameList.Add(c);
        }
        foreach (var c in scores)
        {
            scoreList.Add(c);
        }
        for (int i = 0; i < nameList.Count; i++)
        {
            nameText[i].text = nameList[i];
            scoreText[i].text = scoreList[i].ToString();
        }
    }
}
