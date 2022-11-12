using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum Type { Player, Identity, Interactable, Environment};
    private AudioManager() { }
    [SerializeField] ScriptableObj PlayerObj = null;
    [SerializeField] ScriptableObj IdentityObj = null;
    [SerializeField] ScriptableObj InteractableObj = null;
    [SerializeField] ScriptableObj EnviromentObj = null;

    private Dictionary<string, AudioClip> PlayerClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> IdentiyuClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> InteractableClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> EnviromentClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        AddAllClipsToDic(PlayerObj);
        AddAllClipsToDic(IdentityObj);
        AddAllClipsToDic(InteractableObj);
        AddAllClipsToDic(EnviromentObj);
    }

    /// <summary>
    /// Add the clips held by the scriptable object to the Dictionary
    /// </summary>
    /// <param name="obj"> A scriptable object that contains audio clips. </param>
    private void AddAllClipsToDic(ScriptableObj obj)
    {
        switch (obj.name)
        {
            case "Player":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    PlayerClips.Add(PlayerObj.Sounds[i].name, PlayerObj.Sounds[i]);
                }
                break;

            case "Identity":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    IdentiyuClips.Add(IdentityObj.Sounds[i].name, IdentityObj.Sounds[i]);
                }
                break;

            case "Interactable":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    InteractableClips.Add(InteractableObj.Sounds[i].name, InteractableObj.Sounds[i]);
                }
                break;

            case "Environment":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    EnviromentClips.Add(EnviromentObj.Sounds[i].name, EnviromentObj.Sounds[i]);
                }
                break;
        }
    }

    /// <summary>
    /// return requested Audioclip for interactableObj
    /// </summary>
    /// <param name="soundType"> The type containing the audio clip to request. </param>
    /// <param name="clipName"> The name of requested audioclip </param>
    /// <returns></returns>
    public AudioClip GetClip(Type soundType, string clipName)
    {
        return CheckType(soundType).ContainsKey(clipName) ? CheckType(soundType)[clipName] : null;
    }

    /// <summary>
    /// Return the corresponding Dictionary via type
    /// </summary>
    /// <param name="soundType"> The type containing the audio clip to request. </param>
    /// <returns></returns>
    private Dictionary<string, AudioClip> CheckType(Type soundType)
    {
        Dictionary<string, AudioClip> curdic = new Dictionary<string, AudioClip>();
        switch (soundType)
        {
            case Type.Identity:
                curdic = IdentiyuClips;
                break;

            case Type.Player:
                curdic = PlayerClips;
                break;

            case Type.Interactable:
                curdic = PlayerClips;
                break;

            case Type.Environment:
                curdic = PlayerClips;
                break;
        }

        if (curdic == null)
        {
            Debug.LogError("The type does not exist");
            return null;
        }
        else return curdic;
    }
}
