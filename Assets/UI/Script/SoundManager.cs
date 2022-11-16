using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    GameObject SoundMenu = null;

    public void Awake()
    {
        SoundMenu = GameObject.Find("Option");
    }


    public void Start()
    {
        SoundMenu.SetActive(false);
    }

    // Volume Controller
    public void AudioVolume(float Vol)
    {
        audioSource.volume = Vol;
    }    
}
