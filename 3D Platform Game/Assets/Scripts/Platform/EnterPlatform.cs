using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPlatform : MonoBehaviour
{

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider collider)
    {
        collider.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider collider)
    {
        collider.transform.SetParent(null);   
    }
}
