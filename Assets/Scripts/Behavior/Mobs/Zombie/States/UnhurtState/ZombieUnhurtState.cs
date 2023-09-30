using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class ZombieUnhurtState : IZomebieState
{
    [SerializeField]
    bool test;
    public virtual void DoStateFixedUpdate(SlimeStateMachine stateMachine)
    {
        throw new System.NotImplementedException();
    }

    public virtual void DoStateUpdate(SlimeStateMachine stateMachine)
    {
        if (PhotonNetwork.IsMasterClient != true && PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            return;
        }

        if (stateMachine.GetIsDamaged() == true)
        {
            //Debug.LogError("AAA");
            stateMachine.SetIsDamaged(false);
            stateMachine.SetState(stateMachine.hurtState);
        }
    }

    public virtual void ResetState(SlimeStateMachine stateMachine)
    {
        throw new System.NotImplementedException();
    }
}
