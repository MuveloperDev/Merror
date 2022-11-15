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

public partial class Interactable : MonoBehaviour
{
    public Interactable() { }

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
        Mirror,
        PuzzleDecryption,
        PuzzleFreemasonCipher,
        Door,
    }

    public bool IsLocked = false;
    [SerializeField] protected ObjectType myType = ObjectType.None;
    public ObjectType GetMyType() => myType;

    private delegate void Do();
    private Do DoMyWork = null; // My Interaction

    private void Awake()
    {
        NonInteractable();
    }

    public virtual void Start()
    {
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
        if (Special)
        {
            DoMyWork += Do_Special;
        }
    }
    /// <summary>
    /// Interact with object. Like rotation, getting an item, play sound, do something.
    /// </summary>
    public virtual void Do_Interact()
    {
        if (IsLocked == true) return;
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
