using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZomebieState
{
    public void DoStateUpdate(SlimeStateMachine stateMachine);

    public void DoStateFixedUpdate(SlimeStateMachine stateMachine);

    public void ResetState(SlimeStateMachine stateMachine);
}
