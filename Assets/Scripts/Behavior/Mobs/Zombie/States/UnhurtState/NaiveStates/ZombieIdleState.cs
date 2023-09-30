using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class ZombieIdleState : ZombieNaiveState
{
    [SerializeField]
    float idleTime = 0;

    [SerializeField]
    bool enteredState = false;

    public override void DoStateFixedUpdate(SlimeStateMachine stateMachine)
    {
        stateMachine.movementBehavior.ApplyVelocity();
    }

    public override void DoStateUpdate(SlimeStateMachine stateMachine)
    {
        if (PhotonNetwork.IsMasterClient != true && PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            return;
        }
        base.DoStateUpdate(stateMachine);

        stateMachine.movementBehavior.moveInput.Stop();
        stateMachine.movementBehavior.CalculateVelocity();

        if(enteredState == false)
        {
            idleTime = Random.Range(4, 6);
            enteredState = true;
        }
        idleTime -= Time.deltaTime;
        if(idleTime <= 0)
        {
            enteredState = false;
            stateMachine.SetState(stateMachine.wanderState);
        }
    }
}
