using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesSetsProperties : MonoBehaviour
{
    [SerializeField] float timeDelayed;
    private float _timeDelayedPassed;

    public bool _delayMark;
    // Start is called before the first frame update
    void Start()
    {
        _delayMark = false;
        _timeDelayedPassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_delayMark)
        {
            if(_timeDelayedPassed < timeDelayed)
            {
                _timeDelayedPassed += Time.deltaTime;
            }
            else
            {
                _delayMark = false;
                _timeDelayedPassed = 0;
            }
            
        }
    }
}
