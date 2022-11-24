using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class IdentitiesManager : MonoBehaviour
{
	[Header("Current Identity")]
    [SerializeField] private IdentityStateMachine identity = null;
	[SerializeField] private GameObject[] identities;

	private Vector3[] offsetPos = { new Vector3(18f, 1.5f, 1.4f) };
    private bool isEnable = false;
    public bool IsEnable { get { return isEnable; } set { isEnable = value; } }
    public IdentityStateMachine GetIdentity() => identity;
	public void InstIdentity()
	{
		this.identity = Instantiate(identities[GameManager.Instance.ChapterNum - 1], offsetPos[GameManager.Instance.ChapterNum - 1], Quaternion.identity).GetComponent<IdentityStateMachine>();
        identity.gameObject.SetActive(false);
    }
    
	public void ChaseIdentity()
	{
        identity.transform.position = offsetPos[GameManager.Instance.ChapterNum-1];
		identity.gameObject.SetActive(true);
        isEnable = true;
        Debug.Log(isEnable);
        identity.TurnOnState(BaseStateMachine.State.CHASE);
    }

    public void WaitForChaseIdentity(float time) => Invoke(nameof(ChaseIdentity), time);
    public void ChaseIdentity(Vector3 pos)
    {
        Debug.Log(isEnable);
        identity.transform.position = pos;
        identity.gameObject.SetActive(true);
        isEnable = true;
        identity.TurnOnState(BaseStateMachine.State.CHASE);
    }

    public void OnEnableIdentity(Vector3 pos, Quaternion quaternion, BaseStateMachine.State state)
    {
        identity.transform.position = pos;
        identity.transform.rotation = quaternion;
        identity.gameObject.SetActive(true);
		if (state == BaseStateMachine.State.SLEEPING) identity.GetComponent<NavMeshAgent>().enabled = false;
        identity.TurnOnState(state);
    }
}
