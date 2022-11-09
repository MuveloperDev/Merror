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

        if (Input.GetMouseButtonDown(0))
        {
            ZoomOut();
        }
    }


    private void ZoomIn()
    {
        TimeControl.Pause();
        isZoomIn = true;
        HintCanvas.gameObject.SetActive(true);
    }

    private void ZoomOut()
    {
        TimeControl.Play();
        isZoomIn = false;
        HintCanvas.gameObject.SetActive(false);
    }
}
