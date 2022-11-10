using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;

public class ZoomInHint : MonoBehaviour
{
    [SerializeField] private Transform MainParent = null;
    [SerializeField] private Canvas HintCanvas = null;
    private bool isZoomIn = false;

    private void Update()
    {
        if (isZoomIn == false)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ZoomOut();
        }
    }

    private void ZoomIn()
    {
        HintCanvas.gameObject.SetActive(true);
        TimeControl.Pause();
        isZoomIn = true;
    }

    private void ZoomOut()
    {
        HintCanvas.gameObject.SetActive(false);
        isZoomIn = false;
        TimeControl.Play();
    }
}
