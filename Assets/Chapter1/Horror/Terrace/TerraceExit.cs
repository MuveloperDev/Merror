using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TerraceExit : MonoBehaviour
{
    private Action exitCallback;
    public Action ExitCallback { get { return exitCallback; } set { exitCallback = value; } }

    [SerializeField] private Interactable terraceDoor = null;
    private WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(3f);
    private Coroutine myCo = null;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            myCo ??= StartCoroutine(CheckExit());
        }
    }
    private IEnumerator CheckExit()
    {
        while(true)
        {
            if (terraceDoor.GetUsedState() == false)
            {
                // Isabel scream sound play
                // Isable knocking door sound play
                yield return waitTime;
                // Isabel setactive false sendmessage
                if (exitCallback != null) exitCallback();
                // Disable identity
                Debug.Log("This");
                GameManager.Instance.GetIdentityManager().GetIdentity().TurnOffState();
                GameManager.Instance.GetIdentityManager().GetIdentity().gameObject.SetActive(false);
                myCo = null;
                yield break;
            }
            exitCallback = null;
            yield return null;
        }
    }
}
