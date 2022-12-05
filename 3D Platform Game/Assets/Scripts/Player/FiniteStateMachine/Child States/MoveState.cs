using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : GroundState
{
    Vector3 moveDirection;
    public Vector3 slopMoveDirection;
    public MoveState(Player player, StateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.playerAnimator.SetBool(animationName, true);
        Debug.Log("Enter MoveState");
        
    }

    public override void Exit()
    {
        base.Exit();
         
        player.playerAnimator.SetBool(animationName, false);
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        if (movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else if (player.inputHandler.isDash && player.amountOfDashLeft > 0)
        {
            stateMachine.ChangeState(player.dashState);
        } 
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
        player.StartCoroutine(player.PlayerRotationT(movementInput));
        Move(player.movementDirection);
        
    }


    public void SlopMoveDirection()
    {
        slopMoveDirection = Vector3.ProjectOnPlane(player.movementDirection, player.slopeHit.normal).normalized;
        Debug.DrawRay(player.slopeHit.point, slopMoveDirection, Color.black);
    }

    private void Move(Vector3 move)
    {
        Vector3 playerHorizontalVelocity = player.playerRB.velocity;

        playerHorizontalVelocity.y = 0f;

        Vector3 finalSpeed = player.movementSpeed * Time.deltaTime * move;

        Vector3 speedDif = finalSpeed - playerHorizontalVelocity;

        float accelRate = 5;

        Vector3 movement = accelRate * speedDif;

        player.playerRB.AddForce(movement, ForceMode.Acceleration);
    } 
}
