using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
public partial class Player : MonoBehaviour
{
    /// <summary>
    /// Initialize movement input values and calculate movement boolean values based on input.
    /// Furthermore, Adjust player speed and stamina values.
    /// </summary>
    private void InitMovementValues()
    {
        Horizontal = Input.GetAxis("Horizontal"); // Input horizontal value.
        Vertical = Input.GetAxis("Vertical"); // Input vertical value.
        // Calculate move state
        IsMove = !Mathf.Approximately(Vertical, 0f) || !Mathf.Approximately(Horizontal, 0f);
        // Run state & Crouch State
        IsRun = GameInput.LeftShift;
        IsCrouch = GameInput.LeftCtrl;
        
        MovementLogic();
    }
    /// <summary>
    /// Actually do the movement.
    /// Calculate movement vector and translate player position by rigidbody.MovePosition.
    /// Additionally, Play movement animation.
    /// </summary>
    private void Move()
    {
        Anim_Move(); // Play animations
        if (IsMove) // When only have input
        {
            MoveVector = (Vertical * transform.forward + Horizontal * transform.right).normalized;
            //_Rigidbody.position += Speed * Time.deltaTime * MoveVector;
            _Rigidbody.MovePosition(Speed * Time.deltaTime * MoveVector + _Rigidbody.position);

            if(!_AudioSource.isPlaying)
                PlaySound(walkClip);
        }
        else
            _AudioSource.Stop();
    }
    /// <summary>
    /// Crouch player. This crouching method is that the camera postion translate to under crouched position.
    /// Additionally, Play crouching animations.
    /// </summary>
    private bool isSitting = false;
    public bool IsSitting { get { return isSitting; } }
    private void Crouch()
    {
        Anim_Crouch();
        if (IsCrouch)
        {
            if (StandUpCoroutine != null) // Is already running stand up coroutine?
            {
                StopCoroutine(StandUpCoroutine); // Then, stop.
                StandUpCoroutine = null;
            }
            isSitting = true;
            Debug.Log("isSItting : " + isSitting);
            // Translate camera to crouched position.
            _MainCam.transform.position = Vector3.Lerp(_MainCam.transform.position, CrouchCamPos.position, Time.deltaTime * 6f);
        }
        else if(GameInput.LeftCtrlUp) // When cancel crouching. Called only once when key released.
        {
            if (StandUpCoroutine != null) // Is already running stand up coroutine?
            {
                StopCoroutine(StandUpCoroutine); // Then, stop.
                StandUpCoroutine = null;
            }
            isSitting = false;
            Debug.Log("isSItting : " + isSitting);
            StandUpCoroutine = StartCoroutine(StandUp()); // Run stand up coroutine.
            Debug.Log("Start Stand Up - Coroutine State : " + StandUpCoroutine);
        }
    }
    private IEnumerator StandUp()
    {
        while (true)
        {
            // If main camera Y position is larger than original Y position,
            if (_MainCam.transform.position.y > OriginCamPos.position.y)
            {
                _MainCam.transform.position = OriginCamPos.position; // Fix Y position.
                StandUpCoroutine = null; // Initialize.
                Debug.Log("Finish Stand Up - Coroutine State : " + StandUpCoroutine);
                yield break;
            }
            // Translate main camera from current position to plus Y axis.
            _MainCam.transform.position += Vector3.up * 0.05f;
            yield return null;
        }
    }
    /// <summary>
    /// Rotate Y axis of player by mouse X axis input value.
    /// And rotate X axis of player view camera by mouse Y axis input value.
    /// </summary>
    private void RotatePlayer()
    {
        transform.Rotate(new Vector3(0f, GameInput.MouseX, 0f)); // Player Y
        _MainCam.transform.eulerAngles = new Vector3(
            -GameInput.Clamped_Delta_Mouse_Y, transform.eulerAngles.y, _MainCam.transform.eulerAngles.z);
    }
    private void RotatePlayerSpine()
    {
        Spine.transform.eulerAngles = new Vector3(-6.638f + -GameInput.Clamped_Delta_Mouse_Y, Spine.transform.eulerAngles.y, Spine.transform.eulerAngles.z);
    }
    /// <summary>
    /// Adjust player movement speed and stamina gage. Control coroutines adjusting values include camera shaking.
    /// </summary>
    private void MovementLogic()
    {
        if (Stamina < 100f)
        {
            Debug.Log(Stamina);
        }
        // Not pressed shift and stamina value is in right range.
        // => Player is not running and stamina is not full.
        if (IsRun == false && Stamina < 100f)
        {
            if (StaminaDecreaseCoroutine != null) // If decrease coroutine already running,
            {
                StopCoroutine(StaminaDecreaseCoroutine); // Stop decreasing.
                Debug.Log("Stop Decrease");
                StaminaDecreaseCoroutine = null;
            }
            StaminaRecoverCoroutine ??= StartCoroutine(RecoverStamina()); // Then, recover.
        }
        if (IsCrouch) // Player is crouching
        {
            Speed = MaxCrouchSpeed; // Set speed to crouched speed.
            _AudioSource.pitch = Speed * 0.6f;
            return; // Crouching is primary.
        }
        if (IsMove) // Player has movement.
        {
            Speed = MaxWalkSpeed;
            _AudioSource.pitch = Speed * 0.33f;
            if (IsRun && Stamina > 0f) // Player has movement, is running and have enough stamina.
            {
                RunningShakeCoroutine ??= StartCoroutine(RunningCamShake());
                Speed = MaxRunSpeed; // Set speed to running speed.
                _AudioSource.pitch = Speed * 0.33f;
                if (StaminaRecoverCoroutine != null) // If recover coroutine already running,
                {
                    StopCoroutine(StaminaRecoverCoroutine); // Stop recover.
                    Debug.Log("Stop Recover");
                    StaminaRecoverCoroutine = null;
                }
                StaminaDecreaseCoroutine ??= StartCoroutine(DecreaseStamina()); // Decrease stamina.
            }
            else
            {
                if (RunningShakeCoroutine != null)
                {
                    StopCoroutine(RunningShakeCoroutine);
                    RunningShakeCoroutine = null;
                    _MainCam.transform.localPosition = OriginCamPos.localPosition;
                }
            }
        }
    }
    private IEnumerator DecreaseStamina()
    {
        while (true)
        {
            if (Cheat)
            {
                Stamina = 100f;
            }
            else
            {
                if (Stamina <= 100f)
                {
                    Stamina -= StaminaDecreaseRate * Time.deltaTime;
                    GameManager.Instance.GetUI().UpdateStamina(Stamina);
                    if (Stamina < 0f)
                    {
                        Stamina = 0f;
                        GameManager.Instance.GetUI().UpdateStamina(Stamina);
                        Debug.Log("Out Of Stamina");
                        yield break;
                    }
                }
            }

            yield return null;
        }
    }
    private IEnumerator RecoverStamina()
    {
        while (true)
        {
            Stamina += StaminaRecoverRate * Time.deltaTime;
            GameManager.Instance.GetUI().UpdateStamina(Stamina);
            if (Stamina >= MaxStamina)
            {
                Stamina = MaxStamina;
                GameManager.Instance.GetUI().UpdateStamina(Stamina);
                Debug.Log("Finished Recover Stamina");
                yield break;
            }
            yield return null;
        }
    }

    float Xradius = 0.2f;
    float Yradius = 0.1f;
    private IEnumerator RunningCamShake()
    {
        float degree = 90f;
        bool ShakeDirectionChanged = false;
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                ShakeDirectionChanged = !ShakeDirectionChanged;
                timer = 0f;
            }

            if(ShakeDirectionChanged == false) // Shake to left
            {
                degree += Time.deltaTime * 200f;
            }
            else
            {
                degree -= Time.deltaTime * 200f;
            }

            float x = Mathf.Cos(Mathf.Deg2Rad * degree) * Xradius;
            float y = Mathf.Sin(Mathf.Deg2Rad * degree) * Yradius;

            _MainCam.transform.localPosition = new Vector3(x, y, 0) + OriginCamPos.localPosition;

            yield return null;
        }
    }
}
