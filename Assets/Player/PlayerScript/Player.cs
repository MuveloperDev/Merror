using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using UnityEngine.AI;

public partial class Player : MonoBehaviour
{
    [SerializeField] private CameraState cameraState;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InitPlayer();
        GameInput.Clamped_Delta_Mouse_Y = 0f;

        GameManager.Instance.GetVideoPlayer().CallPlayVideo(
               GameManager.Instance.GetVideoPlayer().getVideoClips.getChapter1.OP,
               () =>
               { Debug.Log("op"); });
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Update()
    {
        InitMovementValues();

        if(Time.timeScale != 0)
            myRay.StartRay(_MainCam.transform, RayDistance, Input.GetMouseButtonDown(0));
        
        GameManager.Instance.ToggleInventory();

        if(GameManager.Instance.ShowInven == true)
        {
            GameManager.Instance.GetInventory().NextItem();
            GameManager.Instance.GetInventory().PrevItem();
        }
        
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
        cameraState.TurnOnState(CameraState.CamState.DEATH);
        GameManager.Instance.GetIdentityManager().GetIdentity().WaitForDisable(6f);
    }
}
