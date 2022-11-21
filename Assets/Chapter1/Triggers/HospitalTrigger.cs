using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalTrigger : MonoBehaviour
{
    [SerializeField] private GameObject hospitalRoom = null;
    [SerializeField] private GameObject wall = null;
    [SerializeField] private AudioSource audioSource = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            wall.SetActive(true);
            hospitalRoom.SetActive(false);
            audioSource.Play();
            GameManager.Instance.Save();
            Invoke("DisableObj", 2f);
        }
    }

    void DisableObj() => gameObject.SetActive(false);

    public void LoadHospital()
    {
        wall.SetActive(true);
        hospitalRoom.SetActive(false);
        DisableObj();
    }
}
