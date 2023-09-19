using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMoveState : IPlayerState
{
    public void DoStateFixedUpdate(PlayerStateMachine stateMachine)
    {
        stateMachine.movementBehavior.ApplyVelocity();
    }

    public void DoStateUpdate(PlayerStateMachine stateMachine)
    {
        stateMachine.movementBehavior.moveInput.UpdateInput();
        stateMachine.movementBehavior.CalculateVelocity();

        PlayAnimation(stateMachine);

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            //return stateMachine.idleState;
            StopAnimation(stateMachine);
            stateMachine.SetState(stateMachine.idleState);
        }
        //return stateMachine.moveState;
    }

    private void PlayAnimation(PlayerStateMachine stateMachine)
    {
        stateMachine.animator.SetBool("Moving", true);

        stateMachine.animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));

        stateMachine.animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
    }

    private void StopAnimation(PlayerStateMachine stateMachine)
    {
        stateMachine.animator.SetBool("Moving", false);

        stateMachine.animator.SetFloat("Horizontal", 0);

        stateMachine.animator.SetFloat("Vertical", 0);
    }
}
