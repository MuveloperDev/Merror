using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSoundController : MonoBehaviour
{
    [SerializeField] private Interactable interactable;
    [SerializeField] private AudioSource audioSource;

    private void OnEnable() => Init();
    void Init()
    { 
        interactable = GetComponent<Interactable>();
        audioSource = GetComponent<AudioSource>();
    }

    
}
