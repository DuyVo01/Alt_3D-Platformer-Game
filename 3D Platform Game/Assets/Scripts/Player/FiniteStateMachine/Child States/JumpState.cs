using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AbilityState
{
    float jumpStartTime;
    public JumpState(Player player, StateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter JumpState");
        player.playerAnimator.SetBool(animationName, true);
        player.amountOfJumpLeft--;
        jumpStartTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        player.playerAnimator.SetBool(animationName, false);
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
        Jump();
        if(Time.time >= jumpStartTime + 0.1f)
        {
            isAbilityDone = true;
        }
        
    }

    private void Jump()
    {
        player.playerRB.velocity = new Vector3(player.currentVelocity.x, 0, player.currentVelocity.z);

        Vector3 jumpDirection = player.jumpHeight * Time.deltaTime * Vector3.up;
        player.playerRB.AddForce(jumpDirection, ForceMode.VelocityChange);
    }

    public void ResetJumpCount() => player.amountOfJumpLeft = 2;
}
