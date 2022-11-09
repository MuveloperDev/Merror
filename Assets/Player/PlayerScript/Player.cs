using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using TMPro;

public partial class Player : MonoBehaviour
{
    private void Start()
    {
        InitPlayer();
        GameInput.Clamped_Delta_Mouse_Y = 0f;
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Update()
    {
        InitMovementValues();
        MyRay.StartRay(_MainCam, RayDistance, Input.GetMouseButtonDown(0));
        RotatePlayer();
        Crouch();
    }
    private void LateUpdate()
    {
        RotatePlayerSpine();
    }
    private void Death()
    {
        Debug.Log("Player : I'm Dead!!!");
    }
}
