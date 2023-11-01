using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public GameManager2 gameManager;
    [SerializeField] TextMeshProUGUI timeScoreText, ConnectedScoreText, totalScoreText;
    [SerializeField] AudioSource gameBGM, gameoverBGM;

    private void Awake() {
        gameBGM.Stop();
        gameoverBGM.Play();
        timeScoreText.text = gameManager.timeScore.ToString();
        ConnectedScoreText.text = gameManager.connectedScore.ToString();
        totalScoreText.text = gameManager.totalScore.ToString();
    }
}
