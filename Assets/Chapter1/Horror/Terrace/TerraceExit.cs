using System;
using System.Collections;
using System.Collections.Generic;
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
            if (terraceDoor.GetUsedState() == true)
            {
                // Isabel scream sound play
                // Isable knocking door sound play
                yield return waitTime;
                // Isabel setactive false sendmessage
                exitCallback();
                myCo = null;
                yield break;
            }
            yield return null;
        }
    }
}
