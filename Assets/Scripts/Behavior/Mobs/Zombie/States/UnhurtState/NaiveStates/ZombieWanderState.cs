using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class ZombieWanderState : IZomebieState
{
    [SerializeField]
    private float wanderTime;

    [SerializeField]
    private bool enteredState = false;

    public void DoStateFixedUpdate(SlimeStateMachine stateMachine)
    {
        Wander(stateMachine);
    }

    public void DoStateUpdate(SlimeStateMachine stateMachine)
    {
        if (PhotonNetwork.IsMasterClient != true && PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            return;
        }

        if(enteredState == false)
        {
            UpdateWanderInput(stateMachine);
            wanderTime = Random.Range(0.25f, 1f);
            enteredState = true;
        }

        wanderTime -= Time.deltaTime;
        if(wanderTime <= 0)
        {
            enteredState = false;
            stateMachine.SetState(stateMachine.idleState);
        }
    }

    public void ResetState(SlimeStateMachine stateMachine)
    {
        
    }

    private void UpdateWanderInput(SlimeStateMachine stateMachine)
    {
        stateMachine.movementBehavior.SetMoveInput(stateMachine.wanderMoveInput);
        stateMachine.movementBehavior.moveInput.UpdateInput();
        stateMachine.movementBehavior.CalculateVelocity();
        stateMachine.GetComponent<Flip>().DoFlipByInput(stateMachine.movementBehavior.moveInput.xInput);
    }

    private void Wander(SlimeStateMachine stateMachine)
    {
        stateMachine.movementBehavior.ApplyVelocity();
    }
}
