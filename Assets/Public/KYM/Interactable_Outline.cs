using EPOOutline;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class Interactable : MonoBehaviour
{
    [Header("::Outline::")]
    [SerializeField] private bool Outlinable = false;
    private Outlinable _Outlinable = null;

    /// <summary>
    /// When player mouse ray enter or exit the object.
    /// </summary>
    /// <param name="value">Outline enabled state</param>
    public virtual void Do_Outline(bool value)
    {
        if (_Outlinable != null)
        {
            _Outlinable.enabled = value;
        }
        //GameManager.Instance.GetUI().ChangeAimIcon(_Outlinable.name, value);
    }
    /// <summary>
    /// Initialize Outline Component. Trying get component and if failed, create new component.
    /// </summary>
    private void InitOutlineComponent()
    {
        //this.AddComponent<Outlinable>(); ==> Never be used. because it can't customizable object by object.
        if (TryGetComponent<Outlinable>(out Outlinable _outlinable)) // Success to get outlinable component
        {
            _Outlinable = _outlinable; // Turn off component at start time.
            _Outlinable.enabled = false;
        }
        else
        {
            Debug.Log("Outline Component Error : There's no outline component.");
            Outlinable = false;
        }
    }
}
