using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    private GameInput _Input = null;
    private Rigidbody _Rigidbody = null;
    private Animator _Animator = null;

    [Header("Player Camera")]
    //[SerializeField] private Camera _MainCam = null;
    [SerializeField] private CinemachineVirtualCamera _MainCam = null;
    [SerializeField] private Transform OriginCamPos = null;
    [SerializeField] private Transform CrouchCamPos = null;

    private Coroutine StandUpCoroutine = null;
    private Coroutine StaminaDecreaseCoroutine = null;
    private Coroutine StaminaRecoverCoroutine = null;
    private Coroutine RunningShakeCoroutine = null;

    [Header("Player Bone")]
    [SerializeField] private Transform Spine = null;

    [Header("Temp")]
    [SerializeField] private UIManager _UI = null;
    [SerializeField] private bool Cheat = false;
    /// <summary>
    /// Initialize player's required components and values.
    /// </summary>
    private void InitPlayer()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        _Animator = GetComponent<Animator>();
        _Input = GetComponent<GameInput>();

        Speed = MaxWalkSpeed;
        Stamina = MaxStamina;
        //_UI.UpdateStamina(Stamina);

        //_MainCam.
    }
}
