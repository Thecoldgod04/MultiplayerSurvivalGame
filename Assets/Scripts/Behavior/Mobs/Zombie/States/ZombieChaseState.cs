using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class ZombieChaseState : IZomebieState
{
    [SerializeField]
    float giveUpTime = 2f;

    [SerializeField]
    Transform targetPlayer;

    bool targetLocked = false;

    public void DoStateFixedUpdate(ZombieStateMachine stateMachine)
    {
        stateMachine.movementBehavior.ApplyVelocity();
    }

    public void DoStateUpdate(ZombieStateMachine stateMachine)
    {
        if (PhotonNetwork.IsMasterClient != true && PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            return;
        }

        if(targetLocked == false)
        {
            PlayerSetup[] targets = GameObject.FindObjectsOfType<PlayerSetup>();
            foreach (PlayerSetup player in targets)
            {
                targetPlayer = FindClosestPlayer(targets, stateMachine);
            }
            stateMachine.movementBehavior.SetMoveInput(stateMachine.GetComponent<FollowTargetMove>());
            stateMachine.GetComponent<FollowTargetMove>().SetTarget(targetPlayer);
            targetLocked = true;
        }

        stateMachine.movementBehavior.moveInput.UpdateInput();
        stateMachine.movementBehavior.CalculateVelocity();

        if(Vector2.Distance(stateMachine.transform.position, targetPlayer.position) > stateMachine.chaseRange)
        {
            giveUpTime -= Time.deltaTime;
            if(giveUpTime <= 0)
            {
                ResetState();
                stateMachine.SetState(stateMachine.idleState);
            }
        }
    }

    private Transform FindClosestPlayer(PlayerSetup[] players, ZombieStateMachine stateMachine)
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
            }
        }

        return closestTransform;
    }

    public void ResetState()
    {
        giveUpTime = 2f;

        targetPlayer = null;

        targetLocked = false;
    }
}
