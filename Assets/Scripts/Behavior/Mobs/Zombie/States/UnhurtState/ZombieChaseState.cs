using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class ZombieChaseState : ZombieUnhurtState
{
    [SerializeField]
    float giveUpTime = 2f;

    [SerializeField]
    Transform targetPlayer;

    bool targetLocked = false;

    public override void DoStateFixedUpdate(SlimeStateMachine stateMachine)
    {
        stateMachine.movementBehavior.ApplyVelocity();
    }

    public override void DoStateUpdate(SlimeStateMachine stateMachine)
    {
        base.DoStateUpdate(stateMachine);

        if (PhotonNetwork.IsMasterClient != true && PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            return;
        }

        if(targetLocked == false || stateMachine.GetComponent<FollowTargetMove>().GetTarget() == null)
        {
            ResetState(stateMachine);
            PlayerSetup[] targets = GameObject.FindObjectsOfType<PlayerSetup>();
            
            targetPlayer = FindClosestPlayer(targets, stateMachine);

            stateMachine.movementBehavior.SetMoveInput(stateMachine.GetComponent<FollowTargetMove>());
            stateMachine.GetComponent<FollowTargetMove>().SetTarget(targetPlayer);
            targetLocked = true;
        }

        stateMachine.movementBehavior.moveInput.UpdateInput();
        stateMachine.movementBehavior.CalculateVelocity();
        stateMachine.GetComponent<Flip>().DoFlipByInput(stateMachine.movementBehavior.moveInput.xInput);

        if (Vector2.Distance(stateMachine.transform.position, targetPlayer.position) > stateMachine.chaseRange)
        {
            giveUpTime -= Time.deltaTime;
            if(giveUpTime <= 0)
            {
                ResetState(stateMachine);
                //stateMachine.SetPlayerTarget(null);
                stateMachine.SetState(stateMachine.idleState);
            }
        }
    }

    private Transform FindClosestPlayer(PlayerSetup[] players, SlimeStateMachine stateMachine)
    {
        Transform closestTransform = null;
        float closestDistance = Mathf.Infinity; // start with max possible distance

        foreach (PlayerSetup t in players)
        {
            float distance = Vector3.Distance(stateMachine.transform.position, t.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTransform = t.transform;
                stateMachine.SetPlayerTarget(t);
            }
        }
        return closestTransform;
    }

    public override void ResetState(SlimeStateMachine stateMachine)
    {
        giveUpTime = 2f;

        targetPlayer = null;

        targetLocked = false;
    }
}
