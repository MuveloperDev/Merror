using EPOOutline;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : MonoBehaviour
{
    [SerializeField] private Transform terrace = null;
    [SerializeField] private CameraState cameraState = null;

    void Interactable()
    {
        terrace.gameObject.SetActive(true);
        Destroy(GetComponent<Interactable>());
        Destroy(GetComponent<Outlinable>());
        cameraState.TurnOnState(CameraState.CamState.PANIC);
        cameraState.callBackPanic = () => { Debug.Log("Call Isabel"); };
        terrace.GetComponentInChildren<TerraceExit>().ExitCallback = () => {
            Destroy(GetComponent<AudioSource>());
        };
    }
}
