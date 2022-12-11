using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputHandler : MonoBehaviour
{
    public static bool isPaused;

    public void OnPaused(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;
    }
}
