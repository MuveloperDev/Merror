using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdentityStateMachine : BaseStateMachine
{
    
    void OnEnable() => Init();

    public override void TurnOnState(State STATE) => base.TurnOnState(STATE);
    public override void TurnOffState() => base.TurnOffState();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TurnOnState(State.CHASE);
        }
    }
    public void WaitForDisable(float time) => Invoke("Disable", time);

    private void Disable() => gameObject.SetActive(false);

}
