using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using TMPro;

public partial class Player : MonoBehaviour
{
    [SerializeField] private CameraState cameraState;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
        myRay.StartRay(_MainCam.transform, RayDistance, Input.GetMouseButtonDown(0));
        GameManager.Instance.ToggleInventory();
        RotatePlayer();
        Crouch();
        Equip();
    }
    private void LateUpdate()
    {
        RotatePlayerSpine();
    }
    private void Death()
    {
        Debug.Log("Player : I'm Dead!!!");
        cameraState.TurnOnState(CameraState.CamState.DEATH);
    }
}
