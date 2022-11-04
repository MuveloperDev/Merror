using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using static CameraState;

public partial class Player : MonoBehaviour
{
    //[SerializeField] private Inventory inventory = null;
    [SerializeField] private CameraState cameraState;

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
        ShootRay(10f);
    }
    private void LateUpdate()
    {
        RotatePlayerSpine();
    }
    private void Death(CamState camState)
    {
        Debug.Log("Player : I'm Dead!!!");
        cameraState.TurnOnState(camState);
    }
    public void StartRay(float maxDistance, bool isClicked)
    {
        Vector3 direction = Camera.main.transform.forward;
        Debug.DrawRay(Camera.main.transform.position, direction * maxDistance, Color.red);
        if (Physics.Raycast(Camera.main.transform.position, direction, out RaycastHit hitInfo, maxDistance))
        {
            Interactable interObj = hitInfo.transform.gameObject.GetComponent<Interactable>();
            if (interObj == null)
                return;

            if (isClicked == true)
            {
                interObj.SendMessage("Do_Interact", SendMessageOptions.DontRequireReceiver);
            }

        }
    }
    public void ShootRay(float maxDistance)
    {
        Vector3 direction = Camera.main.transform.forward;
        Debug.DrawRay(Camera.main.transform.position, direction * maxDistance, Color.red);
        if (Physics.Raycast(Camera.main.transform.position, direction, out RaycastHit hitInfo, maxDistance))
        {
            Debug.Log(hitInfo.collider.name);
            if (Input.GetMouseButtonDown(0))
            {
                Interactable interObj = hitInfo.transform.gameObject.GetComponent<Interactable>();
                if (interObj == null) return;
                interObj.SendMessage("Do_Interact", SendMessageOptions.DontRequireReceiver);
                Debug.Log("Clicked");
            }


        }
    }
}
