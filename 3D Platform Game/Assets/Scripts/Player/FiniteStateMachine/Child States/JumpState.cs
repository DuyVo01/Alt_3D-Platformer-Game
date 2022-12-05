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
        //player.playerRB.velocity = Vector3.zero;
        Vector3 jumpDirection = new Vector3(player.movementDirection.x * player.movementSpeed * 0.2f * Time.deltaTime, player.jumpHeight , player.movementDirection.z * player.movementSpeed * 0.2f * Time.deltaTime);

        player.playerRB.AddForce(jumpDirection - player.playerRB.velocity, ForceMode.VelocityChange);
    }

    public void ResetJumpCount() => player.amountOfJumpLeft = 2;
}
