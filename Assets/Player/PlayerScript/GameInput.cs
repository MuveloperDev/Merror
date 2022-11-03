using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public float MouseX { get; private set; } = 0f;
    public float MouseY { get; private set; } = 0f;
    public float Clamped_Delta_Mouse_Y { get; private set; } = 0f;
    public bool LeftShift { get; private set; } = false;
    public bool LeftShiftDown { get; private set; } = false;
    public bool LeftShiftUp { get; private set; } = false;
    public bool LeftCtrl { get; private set; } = false;
    public bool LeftCtrlUp { get; private set; } = false;

    private void Update()
    {
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        ClampMouseY();

        LeftShift = Input.GetKey(KeyCode.LeftShift);
        LeftShiftDown = Input.GetKeyDown(KeyCode.LeftShift);
        LeftShiftUp = Input.GetKeyUp(KeyCode.LeftShift);

        LeftCtrl = Input.GetKey(KeyCode.LeftControl);
        LeftCtrlUp = Input.GetKeyUp(KeyCode.LeftControl);
    }
    private void ClampMouseY()
    {
        Clamped_Delta_Mouse_Y += Input.GetAxis("Mouse Y"); // Get delta
        // Clamp degree 0 to 360
        Clamped_Delta_Mouse_Y = Clamped_Delta_Mouse_Y > 180f ? Clamped_Delta_Mouse_Y - 360f : Clamped_Delta_Mouse_Y;
        //Debug.Log("Unclamped delta Y : " + Clamped_Delta_Mouse_Y);
        Clamped_Delta_Mouse_Y = Mathf.Clamp(Clamped_Delta_Mouse_Y, -70f, 70f); // Clamp Range : (-Upside, -Downside)
        //Debug.Log("Clamped delta Y : " + Clamped_Delta_Mouse_Y);
    }
}
