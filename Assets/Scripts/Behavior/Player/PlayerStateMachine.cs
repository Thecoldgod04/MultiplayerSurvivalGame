using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerStateMachine : MonoBehaviourPun
{
    private IPlayerState currentState;

    #region States
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    #endregion

    #region Behavior
    public Movement movementBehavior { get; private set; }
    public Flip flipBehavior { get; private set; }

    [field: SerializeField]
    public Animator animator { get; private set; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !photonView.IsMine) return;

        idleState = new PlayerIdleState();
        moveState = new PlayerMoveState();

        movementBehavior = GetComponent<Movement>();
        flipBehavior = GetComponent<Flip>();
        
        currentState = idleState;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !photonView.IsMine) return;
        currentState.DoStateUpdate(this);
    }

    void FixedUpdate()
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !photonView.IsMine) return;
        currentState.DoStateFixedUpdate(this);
    }

    public void SetState(IPlayerState state)
    {
        this.currentState = state;
    }
}
