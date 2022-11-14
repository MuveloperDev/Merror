using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventToiletTrigger : MonoBehaviour
{
    [SerializeField] private GameObject eventObjs = null;
    [SerializeField] private CameraState cameraState = null;
    [SerializeField] private AudioSource[] audios = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("OnCollision");
            eventObjs.gameObject.SetActive(true);
            cameraState.TurnOnState(CameraState.CamState.PANIC);
            cameraState.callBackPanic = () => { eventObjs.gameObject.SetActive(false); };
            gameObject.SetActive(false);
        }
    }


}
