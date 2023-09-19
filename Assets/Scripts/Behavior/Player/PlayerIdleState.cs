using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    public void DoStateFixedUpdate(PlayerStateMachine stateMachine)
    {
        stateMachine.movementBehavior.ApplyVelocity();
    }

    public void DoStateUpdate(PlayerStateMachine stateMachine)
    {
        stateMachine.movementBehavior.moveInput.Stop();
        stateMachine.movementBehavior.CalculateVelocity();
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            //return stateMachine.moveState;
            stateMachine.SetState(stateMachine.moveState);
        }
        //return stateMachine.idleState;
    }
}
