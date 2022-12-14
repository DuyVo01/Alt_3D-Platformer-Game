using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStatus : MonoBehaviour
{
    public int playerScore;
    public bool playerEnhanced = false;

    private float moveSpeed;
    private float jumpHeight;

    Coroutine enchanced;

    public ParticleSystem[] enhancedParticleEffect;

    public Player player;

    public static event Action<PlayerStatus> OnUpdateScore;


    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        moveSpeed = player.movementSpeed;
        jumpHeight = player.jumpHeight;

    }

    private void OnEnable()
    {
        for (int i = 0; i < enhancedParticleEffect.Length; i++)
        {
            enhancedParticleEffect[i].Stop();
        }
        GameLogic.OnGameOver += UpdateFinalScoreOnGameOver;
    }
    private void OnDisable()
    {
        GameLogic.OnGameOver -= UpdateFinalScoreOnGameOver;
    }

    public void UpdateScore()
    {
        OnUpdateScore?.Invoke(this);
    }

    public void UpdateFinalScoreOnGameOver(GameLogic gameLogic)
    {
        gameLogic.finalScoreValue.text = playerScore.ToString("0000");
    }

    public void ScoreCalculating(int scoreToAdd)
    {
        if (playerEnhanced)
        {
            playerScore = playerScore + (scoreToAdd * 2);
        }
        else
        {
            playerScore += scoreToAdd;
        }
    }

    public void ActivateEnhanced() 
    {
        playerEnhanced = true;
        player.movementSpeed = moveSpeed * 1.5f;
        player.jumpHeight = jumpHeight * 1.2f;
        for (int i = 0; i < enhancedParticleEffect.Length; i++)
        { 
            enhancedParticleEffect[i].Play();
        }

        if (enchanced != null)
        {
            StopCoroutine(enchanced);
        }

        enchanced = StartCoroutine(EnchancedCoolDown());
    }

    IEnumerator EnchancedCoolDown()
    {
        yield return new WaitForSeconds(5f);

        for (int i = 0; i < enhancedParticleEffect.Length; i++)
        {
            ParticleSystem.MainModule main = enhancedParticleEffect[i].main;
            
            main.loop = false;

            yield return new WaitForSeconds(main.startLifetime.constant);

            enhancedParticleEffect[i].Stop();

            main.loop = true;
        }

        playerEnhanced = false;

        player.movementSpeed = moveSpeed;

        player.jumpHeight = jumpHeight;
    }
}
