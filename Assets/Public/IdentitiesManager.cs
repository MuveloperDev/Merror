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

	private Vector3 offsetPos = new Vector3(18f, 1.5f, 1.4f);
	public void UpdateIdentity() => identity = identities[GameManager.Instance.ChapterNum-1].GetComponent<IdentityStateMachine>();

	public IdentityStateMachine GetIdentity() => identity;
	public void InstIdentity()
	{
		this.identity = Instantiate(identities[GameManager.Instance.ChapterNum - 1], offsetPos, Quaternion.identity).GetComponent<IdentityStateMachine>();
        identity.gameObject.SetActive(false);
	}

	public void ChaseIdentity()
	{
		identity.transform.position = offsetPos;
		identity.gameObject.SetActive(true);
		identity.TurnOnState(BaseStateMachine.State.CHASE);

    }
    public void OnEnableIdentity(Vector3 pos, BaseStateMachine.State state)
    {
        identity.transform.position = offsetPos;
		
        identity.gameObject.SetActive(true);
        identity.TurnOnState(state);
    }
}
