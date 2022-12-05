using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandState : GroundState
{
    float StartTime;
    public LandState(Player player, StateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.playerRB.AddForce(-player.playerRB.velocity, ForceMode.VelocityChange);
        Debug.Log("Enter Landing");
        player.playerAnimator.SetBool(animationName, true);
        player.animationFinished = false;
        StartTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        player.playerAnimator.SetBool(animationName, false);
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (player.movementDirection != Vector3.zero)
        {
            stateMachine.ChangeState(player.moveState);
        }
        else if(player.animationFinished)
        {
            stateMachine.ChangeState(player.idleState);
        }
        
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
    }
}
