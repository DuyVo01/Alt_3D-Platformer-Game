using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : State
{
    public Vector2 movementInput;
    public bool isJumping;
    public GroundState(Player player, StateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.playerRB.velocity = new Vector3(player.currentVelocity.x, 0f, player.currentVelocity.z);
        player.jumpState.ResetJumpCount();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        movementInput = player.inputHandler.rawMovementInput;
        isJumping = player.inputHandler.isJump;

        if (isJumping && player.isGrounded)
        {
            //player.inputHandler.UsedJump();
            stateMachine.ChangeState(player.jumpState);         
        } else if (!player.isGrounded)
        {
            stateMachine.ChangeState(player.inAirState);
        } 
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
        player.FloatingCollider();
        
    }
}
   

