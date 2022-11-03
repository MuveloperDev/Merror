using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
