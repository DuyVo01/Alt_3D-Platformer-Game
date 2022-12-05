using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 rawMovementInput;
    public bool isJump;
    public bool isDash;
    public bool isHoldingJump;

    public float jumpBufferTime;
    public float jumpBufferPassedTime = 0.2f;
    public float jumpHoldTime;
    public float jumpHoldPassedTime = 0.5f;


    private void Update()
    {
        CheckJump();
        CheckHoldingJump();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        rawMovementInput = context.ReadValue<Vector2>();
        rawMovementInput.Normalize();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isJump = true;
            isHoldingJump = true;
            jumpBufferTime = Time.time;
            jumpHoldTime = Time.time;
        }

        if (context.canceled)
        {
            isHoldingJump = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isDash = true;
        }
        if (context.canceled)
        {
            isDash = false;
        }
    }

    public void UsedJump() => isJump = false;
    public void UsedDash() => isDash = false;

    public void CheckJump()
    {
        if (Time.time > jumpBufferTime + jumpBufferPassedTime && isJump == true)
        {
            isJump = false; 
        }
    }

    public void CheckHoldingJump()
    {
        if (Time.time > jumpHoldTime + jumpHoldPassedTime && isHoldingJump == true)
        {
            isHoldingJump = false;
        }
    }

}
