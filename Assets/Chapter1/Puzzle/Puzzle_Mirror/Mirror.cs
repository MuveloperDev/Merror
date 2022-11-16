using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    private MirrorManager mirrorManager = null;
    private GameObject pieces;
    public bool IsBroken = false;
    private void Start()
    {
        if(IsBroken == true)
        {
            this.gameObject.SetActive(false);
        }
        mirrorManager = GameObject.FindObjectOfType<MirrorManager>();
        pieces = this.transform.GetChild(0).gameObject;
        pieces.SetActive(false);
    }
    /// <summary>
    /// Be Sent Message by player. Break mirror and fall pieces.
    /// </summary>
    private void Break()
    {
        IsBroken = true;
        pieces.SetActive(true);
        pieces.transform.SetParent(null);
        for (int i = 0; i < pieces.transform.childCount; i++)
        {
            pieces.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        mirrorManager.RemoveMirror(this);
        this.gameObject.SetActive(false);
        Invoke("PhysicsOff", 2f);
    }
    /// <summary>
    /// To raise performance, turn off the rigidbodies when falling finished.
    /// </summary>
    private void PhysicsOff()
    {
        for (int i = 0; i < pieces.transform.childCount; i++)
        {
            pieces.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
