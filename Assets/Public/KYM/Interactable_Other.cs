using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public partial class Interactable : MonoBehaviour
{
    [SerializeField] private bool Gettable = false;
    [SerializeField] private bool Special = false;
    [SerializeField] private Light _Light = null;

    /// <summary>
    /// Do something special fuction.
    /// </summary>
    public virtual void Do_Special()
    {
        switch (myType)
        {
            case ObjectType.Toilet_Flush:
                {
                    Do_Flush();
                    break;
                }
            case ObjectType.Switch:
                {
                    Do_Light();
                    break;
                }
        }
    }
    /// <summary>
    /// Flush Toilet water.
    /// </summary>
    protected void Do_Flush()
    {
        if (TryGetComponent<Toilet>(out Toilet toilet))
        {
            StartCoroutine(toilet.Flush());
        }
        else
        {
            Debug.Log("This Object not have Flush Script.");
        }
    }
    /// <summary>
    /// Turn on / off light if this object is switch type.
    /// </summary>
    protected void Do_Light()
    {
        if (_Light != null)
        {
            _Light.enabled = !_Light.enabled;
        }
    }
    /// <summary>
    /// Move this object to player inventory and hide this object.
    /// </summary>
    public virtual void Do_Inventory()
    {
        if (Gettable == false) return;
        switch (myType)
        {
            case ObjectType.None:
                {
                    break;
                }
            case ObjectType.Key:
                {
                    break;
                }
            case ObjectType.Lock:
                {
                    break;
                }
        }
    }
}
