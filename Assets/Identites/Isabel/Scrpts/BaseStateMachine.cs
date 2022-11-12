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
    protected MyRay myRay = null;
    protected Animator myAnimator = null;
    protected NavMeshAgent navMeshAgent = null;
    protected Transform target = null;
    protected AudioSource audioSource = null;
    protected AudioClip myclip = null;

    protected bool isKill = false;
    private State prevState = State.NONE;

    protected virtual void Init()
    {
        myRay ??= new MyRay();
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        myclip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Identity, "Isabel_Run");

        Debug.Log(myclip.name);

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
        isKill = false;
    }


    #region STATE_COURUTINE
    // STATE_COROUTINES

    IEnumerator CHASE_STATE()
    {
        transform.LookAt(target);
        navMeshAgent.enabled = true;
        myAnimator.SetBool(State.CHASE.ToString(), true);
        navMeshAgent.acceleration = 50f;

        while (true)
        {
            yield return new WaitForFixedUpdate();
            myRay.ShootAIRay(transform, 3f, Vector3.up * 0.5f);
            // Distance for catch player
            float desiredDistance = Vector3.Distance(target.transform.position, transform.position);
            if (desiredDistance < 2f)
            {
                if (!isKill)
                {
                    target.SendMessage("Death", CameraState.CamState.DEATH, SendMessageOptions.DontRequireReceiver);
                    isKill = true;
                }
                TurnOffState();
                TurnOnState(State.SCREAM);
            }
            // Reset Destination
            if (navMeshAgent.enabled) navMeshAgent.SetDestination(target.position);

            // Add Ray
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
        yield return new WaitForFixedUpdate();
    }
    IEnumerator DEATH_STATE()
    {
        myAnimator.SetBool(State.DEATH.ToString(), true);
        // Add Audio
        yield return new WaitForFixedUpdate();
    }
    IEnumerator SLEEPING_STATE()
    {
        myAnimator.SetBool(State.SLEEPING.ToString(), true);
        // Add Audio
        yield return new WaitForFixedUpdate();

        while (true)
        {
            yield return new WaitForFixedUpdate();
            float desiredDistance = Vector3.Distance(target.transform.position, transform.position);
            // Need condition for Check player silence mode
            if (desiredDistance < 6f)
            {
                if (!isKill)
                {
                    target.SendMessage("Death", CameraState.CamState.DEATH, SendMessageOptions.DontRequireReceiver);
                    isKill = true;
                }
                TurnOffState();
            }
        }
    }

    #endregion
}
