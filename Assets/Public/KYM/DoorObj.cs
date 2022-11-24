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
        _MyClip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Interactable, "Door_Lock");
        _MySource.clip = _MyClip;
        _MySource.Play();
        Debug.Log("Locked Sound");
    }
}
