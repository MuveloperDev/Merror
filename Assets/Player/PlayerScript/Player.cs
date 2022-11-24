using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public partial class Player : MonoBehaviour
{
    [SerializeField] private CameraState cameraState;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InitPlayer();
        GameInput.Clamped_Delta_Mouse_Y = 0f;

        if(GameManager.Instance.isFirstPlay == true)
        {
            GameManager.Instance.GetVideoPlayer().CallPlayVideo(
               GameManager.Instance.GetVideoPlayer().GetClip(VideoPlayerManager.VideoCategory.CHAPTER1, "OP"),
               () => { Debug.Log("op"); });
        }
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
        GameManager.Instance.GetVideoPlayer().CallPlayVideo(
        GameManager.Instance.GetVideoPlayer().GetClip(VideoPlayerManager.VideoCategory.CHAPTER1, "DEATH"), () => {
            GameManager.Instance.Load();
            SceneManager.LoadSceneAsync("LodingScene");
        });
        GameManager.Instance.GetIdentityManager().GetIdentity().WaitForDisable(6f);
    }
}
