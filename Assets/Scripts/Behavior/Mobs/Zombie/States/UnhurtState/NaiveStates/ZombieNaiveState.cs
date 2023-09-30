using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class ZombieNaiveState : ZombieUnhurtState
{

    public override void DoStateFixedUpdate(SlimeStateMachine stateMachine)
    {

    }

    public override void DoStateUpdate(SlimeStateMachine stateMachine)
    {
        base.DoStateUpdate(stateMachine);

        if (PhotonNetwork.IsMasterClient != true && PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            return;
        }

        if (Physics2D.OverlapCircle(stateMachine.transform.position, stateMachine.detectRange, LayerMask.GetMask("Player")))
        {
            stateMachine.SetState(stateMachine.chaseState);
        }
    }

    public override void ResetState(SlimeStateMachine stateMachine)
    {
        
    }
}
