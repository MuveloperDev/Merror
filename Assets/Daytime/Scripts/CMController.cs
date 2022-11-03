using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMController : MonoBehaviour
{
    [Header("Cinemachine Cams")]
    [SerializeField] private Transform cmvCams = null;
    [SerializeField] private CinemachineVirtualCamera[] cinemachineVirtualCameras = null;

    [Header("Caption Machine")]
    [SerializeField] private CaptionManager captionManager = null;


    private WaitForSecondsRealtime waitForNext = new WaitForSecondsRealtime(6f);

    private void Awake() => cinemachineVirtualCameras = cmvCams.GetComponentsInChildren<CinemachineVirtualCamera>();

    private void Start() => Init();

    private void Init() => StartCoroutine(GoToTablePointOfView());


    // Offset point of view.
    // Go to next Coroutine After going to main virtual camera from previous virtual camera.
    IEnumerator GoToOffsetPointOfView(CinemachineVirtualCamera prevCam, Action nextCoroutine)
    {
        
        cinemachineVirtualCameras[0].enabled = true;
        prevCam.enabled = false;
        yield return waitForNext;
        nextCoroutine();
    }

    // Table point of view.
    // Go to main virtual camera After going to table virtual camera from main virtual camera.
    IEnumerator GoToTablePointOfView()
    {
        yield return new WaitForSecondsRealtime(8f);
        CinemachineVirtualCamera windowCam = cinemachineVirtualCameras[1];
        cinemachineVirtualCameras[0].enabled = false;
        windowCam.enabled = true;
        yield return new WaitForSecondsRealtime(4f);
        captionManager.TurnOnCaption(CaptionManager.Captions.DATE, captionManager.Chapter);
        yield return new WaitForSecondsRealtime(2f);
        captionManager.TurnOffCaption();
        StartCoroutine(GoToOffsetPointOfView(windowCam, delegate { StartCoroutine(GoToMirrorPointOfView()); }));
    }

    // Mirror point of view.
    // Go to main virtual camera After going to mirror virtual camera from main virtual camera.
    IEnumerator GoToMirrorPointOfView()
    {
        captionManager.TurnOffCaption();
        CinemachineVirtualCamera mirrorCam = cinemachineVirtualCameras[2];
        cinemachineVirtualCameras[0].enabled = false;
        mirrorCam.enabled = true;
        yield return waitForNext;
        StartCoroutine(GoToOffsetPointOfView(mirrorCam, delegate { StartCoroutine(GoToWindowPointOfView()); }));
    }

    // Window point of view.
    // Go to main virtual camera After going to window virtual camera from main virtual camera.
    IEnumerator GoToWindowPointOfView()
    {
        CinemachineVirtualCamera windowCam = cinemachineVirtualCameras[3];
        cinemachineVirtualCameras[0].enabled = false;
        windowCam.enabled = true;
        yield return new WaitForSecondsRealtime(4f);
        captionManager.TurnOnCaption(CaptionManager.Captions.REMEMBER, 1);
        yield return new WaitForSecondsRealtime(2f);
        captionManager.TurnOffCaption();
        StartCoroutine(GoToOffsetPointOfView(windowCam, delegate { }));
    }


}
