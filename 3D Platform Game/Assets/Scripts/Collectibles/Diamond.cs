using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public ParticleSystem pickUpParticle;

    private void OnTriggerEnter(Collider other)
    {
        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();

        if(playerStatus != null)
        {
            Instantiate(pickUpParticle, transform.position, transform.rotation);

            playerStatus.ActivateEnhanced();

            playerStatus.UpdateScore();

            gameObject.SetActive(false);
        }
    }
}
