using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

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
    private bool CanLight = false;
    [SerializeField] private GameObject Lighter = null;
    public GameObject GetLight() => Lighter;

    [Header("Temp")]
    [SerializeField] private bool Cheat = false;
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
    }
    private void Equip()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _Animator.SetBool("IsLighter", !Lighter.activeSelf);
            Lighter.SetActive(!Lighter.activeSelf);
        }
    }
}
