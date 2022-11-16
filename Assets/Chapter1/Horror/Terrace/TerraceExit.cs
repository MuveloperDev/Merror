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
    [SerializeField] private AudioSource audioSource = null;
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
                GameManager.Instance.GetIdentityManager().GetIdentity().TurnOffState();
                // Isabel scream sound play
                PlaySound(GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Identity, "Isabel_KnockingDoor"));
                yield return new WaitForSecondsRealtime(1f);
                PlaySound(GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Identity, "Isabel_Bable"));
                yield return new WaitForSecondsRealtime(6f);
                // Isable knocking door sound play
                yield return waitTime;
                // Isabel setactive false sendmessage
                if (exitCallback != null) exitCallback();
                // Disable identity
                GameManager.Instance.GetIdentityManager().GetIdentity().gameObject.SetActive(false);
                myCo = null;
                yield break;
            }
            else GameManager.Instance.GetIdentityManager().GetIdentity().TurnOnState(BaseStateMachine.State.CHASE);
            exitCallback = null;
            yield return null;
        }
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
