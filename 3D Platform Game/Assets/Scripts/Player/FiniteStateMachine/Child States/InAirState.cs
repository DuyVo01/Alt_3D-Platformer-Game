using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : State
{
    private bool isGrounded;
    private bool isHoldingJump;
    private bool isJumping;

    Vector3 playerHorizontalVelocity;
    private Vector2 movementInput;

    public InAirState(Player player, StateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        player.inputHandler.UsedJump();

        player.playerAnimator.SetBool(animationName, true);

        //Get the Velocity from previous state
        playerHorizontalVelocity = player.playerRB.velocity;

        playerHorizontalVelocity.y = 0f;

        Debug.Log("Enter InAirState");
    }

    public override void Exit()
    {
        base.Exit();
        player.playerAnimator.SetBool(animationName, false);
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        isGrounded = player.isGrounded;

        movementInput = player.inputHandler.rawMovementInput;

        isHoldingJump = player.inputHandler.isHoldingJump;

        isJumping = player.inputHandler.isJump;

        if (isGrounded && player.currentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.landState);
        }
        else
        {
            //MovementDirectionWithCamera();

            //rotate the player when moving
             
            if(player.amountOfJumpLeft > 0 && isJumping)
            {
                stateMachine.ChangeState(player.jumpState);
            }

            player.playerAnimator.SetFloat("VelocityY", player.currentVelocity.y);
        }   
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();

        if (movementInput != Vector2.zero)
        {
            player.StartCoroutine(player.PlayerRotationT(movementInput));
        }

        AirMoveAlternative(player.movementDirection);

        if (player.playerRB.velocity.y < 0 && !isGrounded)
        {
            //player.playerRB.velocity += Vector3.up * Physics.gravity.y * (player.fallJumpMultiplier - 1) * Time.deltaTime;
            player.playerRB.AddForce(Vector3.up * Physics.gravity.y * player.fallJumpMultiplier  * Time.deltaTime, ForceMode.VelocityChange);
        }
        if (!isHoldingJump && !isGrounded)
        {
            //player.playerRB.velocity += Vector3.up * Physics.gravity.y * (player.lowJumpMultiplier - 1) * Time.deltaTime;
            player.playerRB.AddForce(Vector3.up * Physics.gravity.y * player.lowJumpMultiplier  * Time.deltaTime, ForceMode.VelocityChange);
        }


    }

    private void AirMoveAlternative(Vector3 move)
    {
        Vector3 finalMove = (player.movementSpeed + 300) * Time.deltaTime * move;

        player.playerRB.AddForce(finalMove, ForceMode.Acceleration);
        
        if (player.playerRB.velocity.magnitude > 10f)
        {
            player.playerRB.AddForce(-finalMove, ForceMode.Acceleration);
        }

    }

}
