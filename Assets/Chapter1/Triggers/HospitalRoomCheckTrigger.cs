using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalRoomCheckTrigger : MonoBehaviour
{
    [SerializeField] private Interactable hospitalRoom = null;
    [SerializeField] private Player player = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            if (player.CanLight) hospitalRoom.IsLocked = false;
        }
    }
}
