using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateMachine : StateMachine
{
    public PState pState;

    private void Start()
    {
        currentState = (IState<StateMachine>) pState;
    }


    private void Update()
    {
        currentState.DoState(this);
    }
}
