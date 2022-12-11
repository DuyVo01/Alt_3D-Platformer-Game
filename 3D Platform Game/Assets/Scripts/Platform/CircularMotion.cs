using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    public Transform pivot;
    private Quaternion originalRotation;

    [SerializeField]
    private float angularSpeed;

    //Update is called once per frame
    void Start()
    {
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.rotation = originalRotation;
        transform.RotateAround(pivot.position, pivot.right, angularSpeed);
    }
}
