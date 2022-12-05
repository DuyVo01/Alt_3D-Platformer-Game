using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    ParticleSystem particleLifeTime;

    private void Awake()
    {
        particleLifeTime = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        ParticleSystem.MainModule main = particleLifeTime.main;
        Destroy(gameObject, main.startLifetime.constant);
    }
}
