using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;

public partial class Player : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Start()
    {
        InitPlayer();
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Update()
    {
        InitMovementValues();
        MyRay.StartRay(_MainCam, 5f, Input.GetMouseButtonDown(0));
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
