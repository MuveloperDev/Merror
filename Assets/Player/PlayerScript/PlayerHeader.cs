using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MyLibrary;
using Unity.VisualScripting;
using System;

public partial class Player : MonoBehaviour
{
    private float Horizontal = 0f;
    private float Vertical = 0f;
    [Header("Player Speed Values")]
    [SerializeField] private float Speed = 1f;
    [SerializeField, Range(0f, 50f)] private float MaxWalkSpeed = 1f;
    [SerializeField, Range(0f, 50f)] private float MaxRunSpeed = 1.5f;
    [SerializeField, Range(0f, 50f)] private float MaxCrouchSpeed = 1f;

    [Header("Player Sprint Stamina Values")]
    private const float MaxStamina = 100f;
    [SerializeField] private float Stamina;
    [SerializeField, Range(0f, 50f)] private float StaminaDecreaseRate = 20f;
    [SerializeField, Range(0f, 50f)] private float StaminaRecoverRate = 10f;

    private bool IsMove = false;
    private bool IsRun = false;
    private bool IsCrouch = false;

    private Vector3 MoveVector;

    private Rigidbody _Rigidbody = null;
    private Animator _Animator = null;
    private MyRay myRay = null;

    [Header("Player Camera")]
    //[SerializeField] private Camera _MainCam = null;
    [SerializeField] private CinemachineVirtualCamera _MainCam = null;
    [SerializeField] private Transform OriginCamPos = null;
    [SerializeField] private Transform CrouchCamPos = null;
    [SerializeField] private float RayDistance = 5f;

    private Coroutine StandUpCoroutine = null;
    private Coroutine StaminaDecreaseCoroutine = null;
    private Coroutine StaminaRecoverCoroutine = null;
    private Coroutine RunningShakeCoroutine = null;

    [Header("Player Bone")]
    [SerializeField] private Transform Spine = null;

    [Header("Player Item")]
    public bool CanLight = false;
    [SerializeField] private GameObject Lighter = null;
    [SerializeField] private GameObject GetLighter = null;
    [SerializeField] private GameObject LighterMesh = null;
    public GameObject GetLight() => Lighter;

    [Header("Temp")]
    [SerializeField] private bool Cheat = false;
    [SerializeField] private GameObject Leg = null;
    [SerializeField] private GameObject Neck = null;

    [Header("Player Audio")]
    [SerializeField] private AudioSource _AudioSource = null;
    [SerializeField] private AudioClip lighterOpenClip = null;
    [SerializeField] private AudioClip lighterCloseClip = null;
    [SerializeField] private AudioClip walkClip = null;
    [SerializeField] private AudioClip roughBreathClip = null;
    /// <summary>
    /// Initialize player's required components and values.
    /// </summary>
    private void InitPlayer()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        _Animator = GetComponent<Animator>();
        myRay ??= new MyRay();

        Speed = MaxWalkSpeed;
        Stamina = MaxStamina;
        Lighter.SetActive(false);
        GetLighter = GameObject.Find("Lighter");

        Lighter.transform.GetChild(0).gameObject.SetActive(false);
        lighterOpenClip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Player, "Lighter_On");
        lighterCloseClip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Player, "Lighter_Close");
        walkClip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Player, "Player_Walk");
        roughBreathClip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Player, "Player_RoughBreathing");
    }
    private void Equip()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && CanLight == true)
        {
            _Animator.SetBool("IsLighter", !LighterMesh.activeSelf);
            PlayLighterSound(!LighterMesh.activeSelf);
            if(!LighterMesh.activeSelf)
                Invoke("DelayActive", 1f);
            else
                LighterMesh.SetActive(!LighterMesh.activeSelf);
        }
    }
    private void PlayLighterSound(bool active)
    {
        if(active == true)
        {
            Lighter.GetComponent<AudioSource>().clip = lighterOpenClip;
            Lighter.GetComponent<AudioSource>().Play();
        }
        else
        {
            Lighter.GetComponent<AudioSource>().clip = lighterCloseClip;
            Lighter.GetComponent<AudioSource>().Play();
        }
    }
    private void PlayWalkSound(AudioClip playClip)
    {
        Leg.GetComponent<AudioSource>().clip = playClip;
        Leg.GetComponent<AudioSource>().Play();
    }
    private void DelayActive() => LighterMesh.SetActive(!LighterMesh.activeSelf);
}
