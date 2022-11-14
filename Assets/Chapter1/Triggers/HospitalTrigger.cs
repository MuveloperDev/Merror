using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalTrigger : MonoBehaviour
{
    [SerializeField] private GameObject hospitalRoom = null;
    [SerializeField] private GameObject wall = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            wall.SetActive(true);
            hospitalRoom.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
