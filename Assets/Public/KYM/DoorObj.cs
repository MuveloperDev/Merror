using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObj : Interactable, ISpecial
{
    public override void Do_Interact()
    {
        if (IsLocked)
        {
            MySpecial();
            return;
        }
        base.Do_Interact();
    }
    public void MySpecial()
    {
        Debug.Log("Locked Sound");
    }
}
