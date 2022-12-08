using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    public Transform pivot;
    private Quaternion originalRotation;
    private Rigidbody RB;

    [SerializeField]private float angle;

    private void Start()
    {
        originalRotation = transform.rotation;
        RB = GetComponent<Rigidbody>();
    }
    //Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.rotation = originalRotation;
        transform.RotateAround(pivot.position, new Vector3(1, 0, 0), angle * Time.deltaTime);
    }
}
