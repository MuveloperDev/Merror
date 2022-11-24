using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventToiletTrigger : MonoBehaviour
{
    [SerializeField] private GameObject eventObjs = null;
    [SerializeField] private CameraState cameraState = null;
    [SerializeField] private AudioSource[] audios = null;
    [SerializeField] private AudioClip myClip = null;
    [SerializeField] private Transform terrace = null;
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.GetIdentityManager().GetIdentity().gameObject.activeSelf)
        {
            Debug.LogError("Already Isabel is Enable by MH");
            return;
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
            {
                eventObjs.gameObject.SetActive(true);
                cameraState.TurnOnState(CameraState.CamState.PANIC);
                PlaySound(audios[0], ClipChanger("2FToilet_Clack"), true);
                PlaySound(audios[1], ClipChanger("2FToilet_Laughing"), false);
                cameraState.callBackPanic = () => {
                    eventObjs.gameObject.SetActive(false);
                    GameManager.Instance.GetIdentityManager().ChaseIdentity();
                };
                terrace.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }

    }
    AudioClip ClipChanger(string clipName) => myClip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.HorrorEvnets, clipName);
    void PlaySound(AudioSource audioSource, AudioClip clip, bool loop)
    {
        Debug.Log(clip.name);
        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
