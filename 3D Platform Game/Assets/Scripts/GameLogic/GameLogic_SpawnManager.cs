using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic_SpawnManager : MonoBehaviour
{
    [Header("Sets of Collectible")]
    [SerializeField] private GameObject[] collectibleSets;

    [Header("Respawn Delay Timer")]
    [SerializeField] private int respawnDelayInSeconds;

    private float _timeDelayPassed = 0;

    private void Update()
    {
        if(_timeDelayPassed < respawnDelayInSeconds)
        {
            _timeDelayPassed += Time.deltaTime;
        }
        else
        {
            Respawn();
            _timeDelayPassed = 0;
        }

        //Debug.Log(collectibleSets[1].GetComponent<CollectiblesSetsProperties>()._delayMark);
    }

    private void Respawn()
    {
        int i = Random.Range(0, collectibleSets.Length - 1);
        
        if (CheckForEmptyCollectibleSets(collectibleSets[i]) && !collectibleSets[i].GetComponent<CollectiblesSetsProperties>()._delayMark)
        {
            RespawnFromEmptyCollectibleSets(collectibleSets[i]);
        }
        
    }

    private bool CheckForEmptyCollectibleSets(GameObject collectibleSet)
    {
        List<GameObject> children = new List<GameObject>();

        foreach(Transform child in collectibleSet.transform)
        {
            if (child.CompareTag("Collectible"))
            {
                children.Add(child.gameObject);
            }
        }

        int countForActive = 0;

        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].activeSelf)
            {
                countForActive += 1;
            }
        }

        if(countForActive == 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void RespawnFromEmptyCollectibleSets(GameObject collectibleSet)
    {
        
            List<GameObject> children = new List<GameObject>();

            foreach (Transform child in collectibleSet.transform)
            {
                if (child.CompareTag("Collectible"))
                {
                    children.Add(child.gameObject);
                }
            }

            for (int i = 0; i < children.Count; i++)
            {
                if (!children[i].activeSelf)
                {
                    children[i].SetActive(true);
                }

            } 
    }
}
