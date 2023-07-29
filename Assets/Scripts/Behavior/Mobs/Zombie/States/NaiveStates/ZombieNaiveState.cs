using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class ZombieNaiveState : IZomebieState
{

    public virtual void DoStateFixedUpdate(ZombieStateMachine stateMachine)
    {

    }

    public virtual void DoStateUpdate(ZombieStateMachine stateMachine)
    {
        if (PhotonNetwork.IsMasterClient != true && PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            return;
        }

        if (Physics2D.OverlapCircle(stateMachine.transform.position, stateMachine.detectRange, LayerMask.GetMask("Player")))
        {
            stateMachine.SetState(stateMachine.chaseState);
        }
    }

    public void ResetState()
    {
        
    }
}
