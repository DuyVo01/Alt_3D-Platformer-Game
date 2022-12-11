using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameLogic : MonoBehaviour
{
    [Header("Time Limit")]
    [SerializeField] private float timeLimit;

    [Header("UI Display")]
    [SerializeField] private TextMeshProUGUI timeCounter;

    [Header("Start Game Menu")]
    [SerializeField] private GameObject StartGameMenu;
    
    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverUI;

    [Header("Scene loaded to replay")]
    [SerializeField] private string sceneToLoad;


    public static event Action<GameLogic> OnGameOver;
    public TextMeshProUGUI finalScoreValue;
    private float _timeRemaining;
    private bool _isInAnotherMenu;
    public static bool isGameOver { private set; get; }
    public static bool isGameStart { get; private set; }

    private void Awake()
    {
        Time.timeScale = 0;
        isGameStart = true;
        StartGameMenu.SetActive(true);
    }

    private void Start()
    {
        isGameOver = false;
        _isInAnotherMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameStart && !isGameOver)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                timeCounter.text = _timeRemaining.ToString("0");
            }
            else if (!_isInAnotherMenu)
            {

                EnterGameOver();
            }
        }
        
    }

    void EnterGameOver()
    {
        Time.timeScale = 0;
        isGameOver = true;
        gameOverUI.SetActive(true);
        OnGameOver?.Invoke(this); 
    }

    public void SetIsInAnotherMenuTrue()
    {
        _isInAnotherMenu = true;
    }

    public void SetIsInAnotherMenuFalse()
    {
        _isInAnotherMenu = false;
    }

    public void SetIsGameStartFalse()
    {
        isGameStart = false;
        Time.timeScale = 1;
    }

    public void ResetValue()
    {
        Time.timeScale = 1;
        isGameOver = false;
        _isInAnotherMenu = false;
        UIInputHandler.isPaused = false;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void TimelimitSetter(string timeLimit)
    {
        this.timeLimit = int.Parse(timeLimit);
        _timeRemaining = this.timeLimit;
        Debug.Log(_timeRemaining);
    }
}
