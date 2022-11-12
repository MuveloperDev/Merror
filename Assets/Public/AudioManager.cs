using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum Type { Identity};
    private AudioManager() { }
    [SerializeField] ScriptableObj PlayerObj = null;
    [SerializeField] ScriptableObj PropsObj = null;
    [SerializeField] ScriptableObj EnviromentObj = null;
    [SerializeField] ScriptableObj IdentityObj = null;

    private Dictionary<string, AudioClip> PlayerClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> PropsClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> EnviromentClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> IdentiyuClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        AddAllClipsToDic(PlayerObj);
        AddAllClipsToDic(PropsObj);
        AddAllClipsToDic(EnviromentObj);
        AddAllClipsToDic(IdentityObj);

        Debug.Log(IdentiyuClips.Count);
    }

    /// <summary>
    /// Add the clips held by the scriptable object to the Dictionary
    /// </summary>
    /// <param name="obj"> A scriptable object that contains audio clips. </param>
    private void AddAllClipsToDic(ScriptableObj obj)
    {
        switch (obj.name)
        {
            case "PlayerSounds":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    PlayerClips.Add(PlayerObj.Sounds[i].name, PlayerObj.Sounds[i]);
                }
                break;

            case "PropsSounds":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    PropsClips.Add(PropsObj.Sounds[i].name, PropsObj.Sounds[i]);
                }
                break;

            case "EnvironmentSounds":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    EnviromentClips.Add(EnviromentObj.Sounds[i].name, EnviromentObj.Sounds[i]);
                }
                break;

            case "Identity":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    IdentiyuClips.Add(IdentityObj.Sounds[i].name, IdentityObj.Sounds[i]);
                }
                break;
        }
    }

    /// <summary>
    /// return requested Audioclip
    /// </summary>
    /// <param name="soundType"> The type containing the audio clip to request. </param>
    /// <param name="clipName"> The name of requested audioclip </param>
    /// <returns></returns>
    public AudioClip GetClip(Interactable.SoundType soundType, string clipName)
    {
        return CheckType(soundType).ContainsKey(clipName) ? CheckType(soundType)[clipName] : null;
    }

    /// <summary>
    /// return requested Audioclip for Player
    /// </summary>
    /// <param name="player">Player param</param>
    /// <param name="clipName">The name of requested audioclip</param>
    /// <returns></returns>
    public AudioClip GetClip(Player player, string clipName)
    {
        return PlayerClips.ContainsKey(clipName) ? PlayerClips[clipName] : null;
    }

    public AudioClip GetClip(Type type, string clipName)
    {
        return CheckType(type).ContainsKey(clipName) ? CheckType(type)[clipName] : null;
    }

    /// <summary>
    /// Return the corresponding Dictionary via type
    /// </summary>
    /// <param name="soundType"> The type containing the audio clip to request. </param>
    /// <returns></returns>
    private Dictionary<string, AudioClip> CheckType(Interactable.SoundType soundType)
    {
        Dictionary<string, AudioClip> curdic = new Dictionary<string, AudioClip>();
        switch (soundType)
        {
            case Interactable.SoundType.Player:
                curdic = PlayerClips;
                break;

            case Interactable.SoundType.Props:
                curdic = PropsClips;
                break;

            case Interactable.SoundType.Enviroment:
                curdic= EnviromentClips;
                break;

            case Interactable.SoundType.Default:
                curdic = IdentiyuClips;
                break;
        }

        if (curdic == null)
        {
            Debug.LogError("The type does not exist");
            return null;
        }
        else return curdic;
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
        }

        if (curdic == null)
        {
            Debug.LogError("The type does not exist");
            return null;
        }
        else return curdic;
    }
}
