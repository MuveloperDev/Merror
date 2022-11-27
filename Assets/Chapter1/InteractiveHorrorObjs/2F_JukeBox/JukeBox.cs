using EPOOutline;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : MonoBehaviour,ISpecial
{
    [SerializeField] private Transform terrace = null;
    [SerializeField] private CameraState cameraState = null;

    public void MySpecial()
    {
        if (GameManager.Instance.GetIdentityManager().GetIdentity().gameObject.activeSelf)
        {
            Debug.LogError("Already Isabel is Enable by MH");
            return;
        }
        Interactable();
    }

    void Interactable()
    {
        if (GameManager.Instance.GetIdentityManager().GetIdentity().gameObject.activeSelf)
        {
            Debug.LogError("Already Isabel is Enable by MH");
            return;
        }
        else
        {
            terrace.gameObject.SetActive(true);
            Destroy(GetComponent<Interactable>());
            Destroy(GetComponent<Outlinable>());
            cameraState.TurnOnState(CameraState.CamState.PANIC);
            // Call Isabel
            cameraState.callBackPanic = () => { GameManager.Instance.GetIdentityManager().ChaseIdentity(); };
            terrace.GetComponentInChildren<TerraceExit>().ExitCallback = () =>
            {
                Destroy(GetComponent<AudioSource>());
            };

        };
    }
}
