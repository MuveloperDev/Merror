using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsabelStateMachin : BaseStateMachine
{
    
    void OnEnable() => Init();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TurnOnState(State.CHASE);
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            TurnOffState();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            TurnOnState(State.SCREAM);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            TurnOnState(State.FOCUS);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            TurnOnState(State.DEATH);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TurnOnState(State.SLEEPING);
        }
    }

    protected override void TurnOnState(State STATE)
    {
        base.TurnOnState(STATE);
    }

    protected override void TurnOffState()
    {
        base.TurnOffState();
    }
}
