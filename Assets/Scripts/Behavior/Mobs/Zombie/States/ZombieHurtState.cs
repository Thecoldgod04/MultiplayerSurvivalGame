using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class ZombieHurtState : IZomebieState
{
    [SerializeField]
    float hurtTime = 0.15f;

    [SerializeField]
    float thrust = 2f;

    [SerializeField]
    float currentHurtTime = 0.15f;

    bool enteredState = false;

    bool isKnocked = false;

    public void DoStateFixedUpdate(ZombieStateMachine stateMachine)
    {
        if(enteredState == false)
        {
            stateMachine.movementBehavior.moveInput.Stop();
            stateMachine.movementBehavior.CalculateVelocity();
            stateMachine.movementBehavior.ApplyVelocity();
            enteredState = true;
        }

        stateMachine.rb.isKinematic = false;
        Vector2 difference = stateMachine.transform.position - stateMachine.GetPlayerTarget().transform.position;
        //Vector2 difference = stateMachine.GetPlayerTarget().transform.position - stateMachine.transform.position;
        difference = difference.normalized * thrust;
        stateMachine.rb.AddForce(difference, ForceMode2D.Impulse);
    }

    public void DoStateUpdate(ZombieStateMachine stateMachine)
    {
        if (PhotonNetwork.IsMasterClient != true && PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            return;
        }

        stateMachine.movementBehavior.SetMoveInput(stateMachine.GetComponent<FollowTargetMove>());
        stateMachine.GetComponent<FollowTargetMove>().SetTarget(null);

        currentHurtTime -= Time.deltaTime;
        if(currentHurtTime <= 0)
        {
            ResetState(stateMachine);
            stateMachine.SetState(stateMachine.idleState);
        }
    }

    public void ResetState(ZombieStateMachine stateMachine)
    {
        stateMachine.rb.velocity = Vector2.zero;
        stateMachine.rb.isKinematic = true;
        currentHurtTime = hurtTime;
        enteredState = false;
    }
}
