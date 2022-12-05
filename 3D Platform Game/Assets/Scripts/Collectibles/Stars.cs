using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stars : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
        
        if(playerStatus != null)
        {
            StartCoroutine(RotateAround(other.gameObject.transform, playerStatus));    
        }
    }

    IEnumerator RotateAround(Transform player, PlayerStatus playerStatus)
    {
        float timePassed = 0;

        while(timePassed < 0.7f)
        {
            timePassed += Time.deltaTime;

            transform.RotateAround((player.position + Vector3.up * 2f) + ((player.position + Vector3.up * 2f) - transform.position) , new Vector3(0, 1, 0), Time.deltaTime * 550f);

            transform.position = Vector3.Lerp(transform.position, player.position + Vector3.up * 0.75f, Time.deltaTime * 2f);

            transform.localScale -= Time.deltaTime * new Vector3(0.8f, 0.8f, 0.8f);

            yield return new WaitForEndOfFrame();
        }

        playerStatus.ScoreCalculating(1);

        playerStatus.UpdateScore();

        gameObject.SetActive(false);
    }
}
