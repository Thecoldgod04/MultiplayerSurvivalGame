using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZomebieState
{
    public void DoStateUpdate(ZombieStateMachine stateMachine);

    public void DoStateFixedUpdate(ZombieStateMachine stateMachine);

    public void ResetState();
}
