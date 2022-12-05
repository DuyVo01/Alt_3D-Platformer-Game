using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    public Player player;
    public StateMachine stateMachine;
    public string animationName;

    public State(Player player, StateMachine stateMachine, string animationName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animationName = animationName;
    }
    public virtual void Enter()
    {
        //player.playerAnimator.SetBool(animationName, true);
        
    }

    public virtual void LogicalUpdate()
    {
        //player.playerAnimator.SetBool(animationName, false);
    }

    public virtual void PhysicalUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
