using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : GroundState
{
    public IdleState(Player player, StateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.playerAnimator.SetBool(animationName, true);
        //player.playerRB.AddForce(-player.playerRB.velocity, ForceMode.VelocityChange);
        Debug.Log("Enter IdleState");
    }

    public override void Exit()
    {
        base.Exit();
        player.playerAnimator.SetBool(animationName, false);
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        if(movementInput.x != 0 || movementInput.y != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
        player.playerRB.AddForce(new Vector3(-player.playerRB.velocity.x * 11, 0, -player.playerRB.velocity.z * 11), ForceMode.Acceleration);
    }
}
