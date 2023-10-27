using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public GameManager2 gameManager;
    [SerializeField] TextMeshProUGUI timeScoreText, ConnectedScoreText, totalScoreText;

    private void Awake() {
        timeScoreText.text = gameManager.timeScore.ToString();
        ConnectedScoreText.text = gameManager.connectedScore.ToString();
        totalScoreText.text = gameManager.totalScore.ToString();
    }
}
