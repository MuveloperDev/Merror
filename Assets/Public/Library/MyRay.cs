using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRay
{
    private RaycastHit? lastHit = null; // Last hit object
    private RaycastHit currentHit; // Current hitting object
    private bool isRayExit = false; // Is my ray point another one?
    private Interactable interObj = null; // Can interactable object
    public Interactable GetInterObj() => interObj;
    public bool IsRayExit { get { return isRayExit; } }

    /// <summary>
    /// Custom Ray(Start Param's Cinemachine Virtual Camera)
    /// </summary>
    /// <param name="pointCamera"> Starting Point Camera </param>
    /// <param name="maxDistance"> Raycast Max distance </param>
    /// <param name="isClicked"> Is input costom Key (Keyboard or Mouse) </param>
    public void StartRay(Transform start, float maxDistance, bool isClicked)
    {
        ShootRay(start, maxDistance, isClicked);
    }
    /// <summary>
    /// Shoot Ray Main Logic
    /// </summary>
    private void ShootRay(Transform startPos, float maxDistance, bool isClicked)
    {
        if (Time.timeScale == 0f)
            return;

        Vector3 offset = Vector3.zero;
        if(Shoot(startPos.position, startPos.forward, maxDistance))
        {
            if (lastHit == null) // When game start at first
            {
                isRayExit = true;
                lastHit = currentHit;
            }
            else // When playing,
            {
                if (isRayExit = !(lastHit.Equals(currentHit))) // Something have changes
                {
                    // Turn off the outline last object's. if it can interactable.
                    if (lastHit.Value.transform.TryGetComponent<Interactable>(out interObj))
                    {
                        interObj.SendMessage("Do_Outline", false, SendMessageOptions.DontRequireReceiver);
                    }
                    // Turn on the outline new object's. if it can interactable.
                    if (currentHit.transform.TryGetComponent<Interactable>(out interObj))
                    {
                        interObj.SendMessage("Do_Outline", true, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            if (isClicked == true && interObj != null) // Click down and can interactable
            {
                // Do something
                interObj.SendMessage("Do_Interact", SendMessageOptions.DontRequireReceiver);
            }
            lastHit = currentHit; // Initialize
        }
        else // Ray didn't hit anything.
        {
            if (lastHit != null) // If have last hit information,
            {
                // Turn off last one.
                if (lastHit.Value.transform.TryGetComponent<Interactable>(out interObj))
                {
                    interObj.SendMessage("Do_Outline", false, SendMessageOptions.DontRequireReceiver);
                }
            }
            isRayExit = true; // Ray is exit.
            lastHit = null; // Initialize
        }
    }
    public void ShootAIRay(Transform startPos, float maxDistance, Vector3 offset)
    {
        if (Time.timeScale == 0f)
            return;
        startPos.position += offset;
        if (Shoot(startPos.position, startPos.forward, maxDistance))
        {
            if (currentHit.transform.gameObject.TryGetComponent<Interactable>(out interObj))
            {
                if (interObj.IsLocked == false)
                {
                    // Door and is closed
                    if (interObj.GetMyType() == Interactable.ObjectType.Door && interObj.GetUsedState() == false)
                    {
                        interObj.SendMessage("Do_Interact", SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
    }
    public void ShootAIRay(Transform startPos, float maxDistance)
    {
        if (Time.timeScale == 0f)
            return;

        if(Shoot(startPos.position, startPos.forward, maxDistance))
        {
            if (currentHit.transform.gameObject.TryGetComponent<Interactable>(out interObj))
            {
                if (interObj.IsLocked == false)
                {
                    if (interObj.GetMyType() == Interactable.ObjectType.Door)
                    {
                        interObj.SendMessage("Do_Interact", SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
    }
    private bool Shoot(Vector3 start, Vector3 end, float distance)
    {
        Debug.DrawRay(start, end * distance, Color.red);
        return Physics.Raycast(start, end, out currentHit, distance); // Hit something
    }
}