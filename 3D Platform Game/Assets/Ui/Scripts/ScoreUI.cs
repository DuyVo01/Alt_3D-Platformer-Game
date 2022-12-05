using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        PlayerStatus.OnUpdateScore += UpdateScoreText;

    }
    private void OnDisable()
    {
        PlayerStatus.OnUpdateScore -= UpdateScoreText;
    }
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScoreText(PlayerStatus playerStatus)
    {
        scoreText.text = playerStatus.playerScore.ToString();
    }
}
