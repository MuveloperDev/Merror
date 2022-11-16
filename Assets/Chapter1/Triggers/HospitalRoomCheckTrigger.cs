using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalRoomCheckTrigger : MonoBehaviour
{
    [SerializeField] private Interactable hospitalRoom = null;
    [SerializeField] private GameObject GetLighter = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.GetInventory().FindInInven(GetLighter) == true)
            {
                hospitalRoom.IsLocked = false;
            }
        }
    }
}
