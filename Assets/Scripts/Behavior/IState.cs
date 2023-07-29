using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T> where T : StateMachine
{
    void DoState(T stateMachine);
}
