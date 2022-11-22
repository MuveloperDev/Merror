using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
#pragma warning disable 0414

public partial class Interactable : MonoBehaviour
{
    protected enum Axis // Rotation Axis
    {
        X, Y, Z
    }
    [Header("::Rotation::")]
    [SerializeField] protected bool Rotatable = false;
    [Header("Rotation Settings")]
    [Tooltip("Settings about object rotation.")]
    [SerializeField] protected Axis Rotation_Axis; // Where to rotate
    [SerializeField] protected float Target_Angle = 90f; // Total rotation range
    [SerializeField] protected float Rotate_Speed = 70f; // Rotation speed rate
    [SerializeField] protected bool Invert = false; // Invert rotation angle
    [Tooltip("The object was rotated at least once.")]
    [SerializeField] protected bool IsUsed = false; // Check this object is used at least once
    public bool GetUsedState() => IsUsed;
    [SerializeField] protected bool AutoReset = false; // Automatically reset rotation
    [SerializeField] protected float ResetTimer = 0f; // Reset Delay
    [SerializeField] protected float Reset_Speed = 50f; // Reseting speed


    protected Coroutine RotateCoroutine = null;
    protected bool FinishReset = false;

    [SerializeField] NavMeshObstacle myobstacle = null;

    public virtual void Do_Rotate() // Can be sent message from player.
    {
        if (Rotatable == false) return;
        RotateCoroutine ??= StartCoroutine(RotateObj()); // If null, start.
    }
    /// <summary>
    /// Rotate Object based on axis set
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator RotateObj()
    {
        float speed = Rotate_Speed;

        if (AutoReset)
        {
            if (FinishReset) // If second 'reset rotation', adjust speed to reset_speed.
            {
                speed = Reset_Speed;
            }
        }
        //Invert = IsUsed == true ? true : false; // If is used, invert rotation
        float deltaRotation = 0f; // Accumulated rotation values
        float framePerRotate = 0f; // Rotation value per 1 frame
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(ResetTimer);
        Debug.Log("Started Rotating Object");

        while (true)
        {
            if (deltaRotation >= Target_Angle) // When rotate enough
            {
                Debug.Log("Finished Rotating Object");
                IsUsed = !IsUsed; // Check used
                if (myobstacle != null)
                    myobstacle.enabled = IsUsed;
                //Invert = IsUsed == true ? false : true;   
                if (AutoReset)
                {
                    if(FinishReset) // Is reset - ing?
                    {
                        FinishReset = !FinishReset; // Reset temp value
                        RotateCoroutine = null; // Reset co
                        yield break;
                    }
                    else // Will be reset?
                    {
                        FinishReset = !FinishReset; // Change temp value
                        yield return delay; // delay
                        RotateCoroutine = null;
                        RotateCoroutine = StartCoroutine(RotateObj()); // Rotate -axis
                        yield break;
                    }     
                }
                RotateCoroutine = null; // Initialize coroutine
                yield break;
            }
            CalculateFramePerRotate(ref framePerRotate, ref speed); // Calculate how much I should rotate
            deltaRotation += Mathf.Abs(framePerRotate); // Accumulate delta angle
            switch (Rotation_Axis) // Check axis
            {
                case Axis.X: { transform.Rotate(new Vector3(framePerRotate, 0f, 0f)); break; }
                case Axis.Y: { transform.Rotate(new Vector3(0f, framePerRotate, 0f)); break; }
                case Axis.Z: { transform.Rotate(new Vector3(0f, 0f, framePerRotate)); break; }
            }
            yield return null;
        }
    }
    /// <summary>
    /// Return angle that should rotate this frame.
    /// </summary>
    /// <param name="angle">Reference of an angle variable : framePerRotate</param>
    /// <returns></returns>
    private float CalculateFramePerRotate(ref float angle, ref float speed)
    {
        angle = Time.deltaTime * speed; // Calculate angle based on delta time and speed
        angle = Invert == true ? -angle : angle; // Should be inverted?
        angle = IsUsed == true ? angle : -angle; // Is used?
        return angle;
    }
}
