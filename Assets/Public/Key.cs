using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private Interactable door;

    private void UnLockDoor()
    {
        Debug.Log(gameObject.name);
        door.IsLocked = false;
    }
}
