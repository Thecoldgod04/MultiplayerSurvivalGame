using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public void DoStateUpdate(PlayerStateMachine stateMachine);

    public void DoStateFixedUpdate(PlayerStateMachine stateMachine);
}
