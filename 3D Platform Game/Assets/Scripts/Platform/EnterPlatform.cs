using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPlatform : MonoBehaviour
{
    public Vector3 lastPlatformPosition;
    public Vector3 velocity;
    private void Start()
    {
        lastPlatformPosition = transform.position;
    }

    private void FixedUpdate()
    {
        velocity = (transform.position - lastPlatformPosition) / Time.deltaTime;

        lastPlatformPosition = transform.position;
    }
}
