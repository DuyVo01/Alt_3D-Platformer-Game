using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameLogic_Score : MonoBehaviour
{
    [Header("Score Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highestScoreText;

    private void OnEnable()
    {
        PlayerStatus.OnUpdateScore += UpdateScoreText;
        PlayerStatus.OnUpdateScore += SetHighScore;

    }
    private void OnDisable()
    {
        PlayerStatus.OnUpdateScore -= UpdateScoreText;
        PlayerStatus.OnUpdateScore -= SetHighScore;
    }

    private void Start()
    {
        highestScoreText.text = PlayerPrefs.GetInt("HighScore").ToString("00");
    }

    public void UpdateScoreText(PlayerStatus playerStatus)
    {
        scoreText.text = playerStatus.playerScore.ToString("00");
    }

    public void SetHighScore(PlayerStatus playerStatus)
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            if(playerStatus.playerScore > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", playerStatus.playerScore);
                highestScoreText.text = PlayerPrefs.GetInt("HighScore").ToString("00");
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
    }

}
