using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class BaseStateMachine : MonoBehaviour
{
    // STATES
    protected enum State
    {
        NONE,
        IDLE,
        RUN,
        SCREAM,
        FOCUS,
        DEATH,
        SLEEPING,
    }

    protected Animator myAnimator = null;
    protected NavMeshAgent navMeshAgent = null;
    protected Transform transform = null;
    protected Transform target = null;
    protected AudioSource audioSource = null;

    private State prevState = State.NONE;
    protected virtual void Init()
    { 
        myAnimator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
    }

    
    // Assign state.
    protected virtual void TurnOnState(State STATE)
    {
        Debug.Log($"prevState : {prevState} paramState : {STATE}");

        // 상태해제 테스트용
        if (prevState == STATE)
        { 
            TurnOffState();
            return;
        }
        else if (prevState != State.IDLE)
        {
            
            TurnOffState();
        }
        prevState = STATE;
        StartCoroutine(STATE.ToString() + "_STATE");
    }

    // Deallocate state.
    protected virtual void TurnOffState()
    {
        if (navMeshAgent != null)
        { 
            navMeshAgent.ResetPath();
            navMeshAgent.enabled = false;
        } 

       
        // False animation of previous state and Stop coroutine.
        if(prevState != State.NONE) myAnimator.SetBool(prevState.ToString(), false);
        StopCoroutine(prevState.ToString() + "_STATE");
        prevState = State.IDLE;
    }


    #region STATE_COURUTINE
    // STATE_COROUTINES

    IEnumerator RUN_STATE()
    {
        if (navMeshAgent != null)
        {
            transform.LookAt(target);
            navMeshAgent.enabled = true;
        }
        if (navMeshAgent == null) navMeshAgent = GetComponent<NavMeshAgent>();
        if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform;
        


        myAnimator.SetBool(State.RUN.ToString(), true);
        navMeshAgent.acceleration = 50f;
        while (true)
        {
            yield return null;
            navMeshAgent.SetDestination(target.position);
        }
    }

    IEnumerator SCREAM_STATE()
    {
        myAnimator.SetBool(State.SCREAM.ToString(), true);
        // Add Audio
        yield return new WaitForSeconds(2f);
        TurnOffState();
    }
    IEnumerator FOCUS_STATE()
    {
        myAnimator.SetBool(State.FOCUS.ToString(), true);
        // Add Audio
        yield return null;
    }
    IEnumerator DEATH_STATE()
    {
        myAnimator.SetBool(State.DEATH.ToString(), true);
        // Add Audio
        yield return null;
    }
    IEnumerator SLEEPING_STATE()
    {
        myAnimator.SetBool(State.SLEEPING.ToString(), true);
        // Add Audio
        yield return null;
    }

    #endregion
}
