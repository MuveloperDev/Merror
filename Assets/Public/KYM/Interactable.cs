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
using MyLibrary;
using System;

public partial class Interactable : MonoBehaviour
{
    [Obsolete]
    public enum ObjectType
    {
        None,
        Key,
        Toilet_Flush,
        Switch,
        PuzzleGuessWho,
        PuzzleIsabellRoom,
        Mirror,
        PuzzleDecryption,
        PuzzleFreemasonCipher,
        Door,
        JukeBox,
        Lighter,
        Hammer,
    }

    public bool IsLocked = false;
    [SerializeField, Obsolete] public ObjectType myType = ObjectType.None;
    [Obsolete] public ObjectType GetMyType() => myType;

    protected delegate void Do();
    protected Do DoMyWork = null; // My Interaction

    private void Awake()
    {
        NonInteractable();
    }

    public virtual void Start()
    {
        if (Special)
        {
            DoMyWork += Do_Special;
        }
        if (Outlinable)
        {
            InitOutlineComponent();
        }
        if (Moveable)
        {
            DoMyWork += Do_Movement;
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
    }
    /// <summary>
    /// Interact with object. Like rotation, getting an item, play sound, do something.
    /// </summary>
    public virtual void Do_Interact()
    {
        if (IsLocked == true)
        {
            if(myType == ObjectType.Door)
            {
                //Play Clip
                _MyClip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Interactable, "Door_Lock");
                _MySource.clip = _MyClip;
                _MySource.Play();
            }
            return;
        } 
        DoMyWork(); // Do delegate chain
    }
    public virtual void Do_Inventory()
    {
        GameManager.Instance.GetInventory().InsertItem(this.gameObject, Inventory_Scale);
    }
    public void NonInteractable()
    {
        if (Special == false) // If this object do not anything special,
        {
            // If this not have any other work
            if ((Moveable || Rotatable || Audio_Playable || Gettable) == false)
            {
                Destroy(this); // Destroy Interactable
                if (Outlinable) // If have outline component
                {
                    if (TryGetComponent<Outlinable>(out Outlinable outline))
                    {
                        Destroy(outline); // Destroy outline
                    }
                    Outlinable = false;
                }
            }
        }
    }
}
