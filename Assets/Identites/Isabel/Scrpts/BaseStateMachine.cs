using System.Collections;
using UnityEngine;
using UnityEngine.AI;



public class BaseStateMachine : MonoBehaviour
{
    // STATES
    protected enum State
    {
        NONE,
        IDLE,
        CHASE,
        SCREAM,
        FOCUS,
        DEATH,
        SLEEPING,
    }

    protected Animator myAnimator = null;
    protected NavMeshAgent navMeshAgent = null;
    protected Transform target = null;
    protected AudioSource audioSource = null;

    protected bool isKill = false;
    private State prevState = State.NONE;

    protected virtual void Init()
    { 
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.enabled = false;
        isKill = false;
    }

    
    // Assign state.
    protected virtual void TurnOnState(State STATE)
    {
        #region error
        if (myAnimator == null)
        {
            Debug.LogError("Please Add Animator Component by MH");
            return;
        }

        if (audioSource == null) 
        {
            Debug.LogError("Please Add audioSource Component by MH");
            return;
        }
        #endregion

        // 상태해제 테스트용
        if (prevState == STATE)
        {
            TurnOffState();
            return;
        }

        prevState = STATE;
        StartCoroutine(STATE.ToString() + "_STATE");
    }



    // Deallocate state.
    protected virtual void TurnOffState()
    {
        if (navMeshAgent != null && navMeshAgent.enabled)
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

    IEnumerator CHASE_STATE()
    {
        if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform;
        transform.LookAt(target);

        navMeshAgent.enabled = true;

        

        myAnimator.SetBool(State.CHASE.ToString(), true);
        navMeshAgent.acceleration = 50f;
        while (true)
        {
            yield return null;
            float desiredDir = Vector3.Distance(target.transform.position, transform.position);
            if (desiredDir < 2f)
            {
                if (!isKill)
                {
                    target.SendMessage("Death", CameraState.CamState.PANIC, SendMessageOptions.DontRequireReceiver);
                    isKill = true;
                }
                TurnOffState();
                TurnOnState(State.SCREAM);
            }
            if (navMeshAgent.enabled) navMeshAgent.SetDestination(target.position);
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
