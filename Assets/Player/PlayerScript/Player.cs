using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    CapsuleCollider _Collider;
    private void Awake()
    {
        //_Collider = GetComponent<CapsuleCollider>();
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
        //DoNotCliming();
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
    private void OnCollisionStay(Collision collision)
    {
        //Debug.DrawRay(transform.position + new Vector3(0f, _Collider.height * 0.5f, 0f), collision.contacts[0].point * 10f, Color.red);
    }
    bool LeftWallCollision = false;
    bool RightWallCollision = false;
    float CheckRange = 1f;
    private void DoNotCliming()
    {
        //new Vector3(0f, _Collider.height, 0f)
        Debug.DrawRay(transform.position, (transform.right + transform.forward).normalized * CheckRange, Color.red);
        if (Physics.Raycast(transform.position, (transform.right + transform.forward).normalized,
            out RaycastHit hitRight, CheckRange))
        {
            RightWallCollision = true;
        }
        else RightWallCollision = false;
        Debug.DrawRay(transform.position, (-transform.right + transform.forward).normalized * CheckRange, Color.red);
        if (Physics.Raycast(transform.position, (-transform.right + transform.forward).normalized,
            out RaycastHit hitLeft, CheckRange))
        {
            LeftWallCollision = true;
        }
        else LeftWallCollision = false;
        if (LeftWallCollision && RightWallCollision)
        {
            Debug.Log("STUCK AT CORNER");
            _Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            _Rigidbody.constraints = RigidbodyConstraints.None;
            _Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }



    }
}
