//***********************************************************************************//
// Author : Kwon Yong Moon                                                           //
// Last Update : 22.11.03                                                            //
// Detail : Add this script to interactable object. It will be able to rotate object //
//          by custom axis and custom angle. And, it shows outline effect when mouse //
//          hover. Plus, play sound if it will be needed.                            //
//***********************************************************************************//

using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Interactable : MonoBehaviour
{
    public Interactable()
    {

    }

    public enum ObjectType
    {
        None,
        Key,
        Lock,
        Toilet_Flush,
        Sink,
        Switch,
        PuzzleGuessWho,
        PuzzleIsabellRoom,
    }
    [SerializeField] protected ObjectType myType = ObjectType.None;

    private delegate void Do();
    private Do DoMyWork = null; // My Interaction

    public virtual void Start()
    {
        if (Outlinable)
        {
            InitOutlineComponent();
        }
        if (Rotatable)
        {
            DoMyWork += Do_Rotate;
        }
        if (Audio_Playable)
        {
            InitAudioSource();
            DoMyWork += Do_PlaySound;
        }
        if (Gettable)
        {
            DoMyWork += Do_Inventory;
        }
        if (Special)
        {
            DoMyWork += Do_Special;
        }
    }
    /*
    public virtual void Update()
    {
        if(Input.GetMouseButtonDown(0)) // Temp condition
        {
            Do_Rotate(); // If null, start.
            Do_Light();
        }
    }*/

    /// <summary>
    /// Interact with object. Like rotation, getting an item, play sound, do something.
    /// </summary>
    public virtual void Do_Interact()
    {
        DoMyWork(); // Do delegate chain
    } 
}
