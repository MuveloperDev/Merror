using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public partial class Interactable : MonoBehaviour
{
    public enum SoundType
    {
        Player,
        Enviroment,
        Props,
        Default,
    }
    [Header("::Audio::")]
    [Tooltip("Is this object have to play any sound?")]
    [SerializeField] private bool Audio_Playable = false;
    [SerializeField] private AudioClip _MyClip = null;
    [SerializeField] private SoundType Type = SoundType.Default;
    [SerializeField] private string Clip_Name = "";

    private AudioSource _MySource = null;

    /// <summary>
    /// Play my audio clip if this object is audio playable object.
    /// </summary>
    public virtual void Do_PlaySound()
    {
        if (Audio_Playable == false) return;
        _MySource.Play();
    }
    /// <summary>
    /// If this object is audio playable object, request my audio clip and intialize component.
    /// </summary>
    private void InitAudioSource()
    {
        if (Audio_Playable == false) return; // This object doesn't need an audio component.

        if(TryGetComponent<AudioSource>(out AudioSource _audioSource))
        {
            _MySource = _audioSource;    
        }
        else { _MySource = this.AddComponent<AudioSource>(); }
        _MySource.playOnAwake = false;

        RequestMyClip();

        if (_MyClip != null)
        {
            _MySource.clip = _MyClip;
        }
    }
    /// <summary>
    /// Request my audio clip from AudioManager.
    /// </summary>
    private void RequestMyClip()// (Param : Type myType, string clip name)
    {
        string desired_Clip = "";
        //_MyClip = AudioManager.Instance.GetClip(Type.myType, string desired_Clip_Name);
        if(_MyClip == null)
        {
            Debug.LogWarning(this.name + " : Failed to get audio clip - " + desired_Clip);
        }
    }
}