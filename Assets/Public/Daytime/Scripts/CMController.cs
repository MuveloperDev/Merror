using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CaptionManager;
using static VideoPlayerManager.VideoClips;

public class CMController : MonoBehaviour
{
    [Header("Cinemachine Cams")]
    [SerializeField] private Transform cmvCams = null;
    [SerializeField] private CinemachineVirtualCamera[] cinemachineVirtualCameras = null;


    [Header("Caption UI")]
    [SerializeField] private CanvasGroup ui_capturePanel = null;
    [SerializeField] private TextMeshProUGUI ui_captionTxt = null;

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
        TurnOnCaption(CaptureManager.Category.REMEMBER, GameManager.Instance.ChapterNum);
        yield return new WaitForSecondsRealtime(2f);
        TurnOffCaption();
        StartCoroutine(GoToOffsetPointOfView(windowCam, delegate { StartCoroutine(GoToMirrorPointOfView()); }));
    }

    // Mirror point of view.
    // Go to main virtual camera After going to mirror virtual camera from main virtual camera.
    IEnumerator GoToMirrorPointOfView()
    {
        TurnOffCaption();
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
        TurnOnCaption(CaptureManager.Category.REMEMBER, GameManager.Instance.ChapterNum);
        yield return new WaitForSecondsRealtime(2f);
        TurnOffCaption();
        StartCoroutine(GoToOffsetPointOfView(windowCam, delegate { SceneManager.LoadSceneAsync("LodingScene"); }));
    }

    // Set text in caption list after Increase caption UI alpha.
    public void TurnOnCaption(CaptureManager.Category capture, int chapter)
    {
        if (ui_captionTxt.text.Length != 0) ui_captionTxt.text = "";
        StartCoroutine(SetCaptionPanelAlpha(1, GameManager.Instance.GetCaptureManager().GetCapture(capture, chapter)));
        chapter++;
    }

    // Set text in caption list after Decrease caption UI alpha.
    public void TurnOffCaption() => StartCoroutine(SetCaptionPanelAlpha(0, ""));


    /// <summary>
    /// Set caption alpha and assign caption text.
    /// </summary>
    /// <param name="alpha">Set the alpha value. Assign only 0 or 1</param>
    /// <param name="caption">Assgin value for caption text</param>
    /// <returns></returns>
    IEnumerator SetCaptionPanelAlpha(int alpha, string caption)
    {
        float value = alpha == 1 ? Time.deltaTime * 6f : -Time.deltaTime * 3f;
        do
        {
            yield return null;
            ui_capturePanel.alpha += value;
        } while (ui_capturePanel.alpha != alpha);
        ui_captionTxt.text = caption;
    }
}
