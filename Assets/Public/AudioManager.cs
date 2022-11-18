using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum Type { Player, Identity, HorrorEvnets, Interactable, Environment, Puzzle};
    private AudioManager() { }
    [SerializeField] ScriptableObj PlayerObj = null;
    [SerializeField] ScriptableObj InteractableObj = null;
    [SerializeField] ScriptableObj EnviromentObj = null;
    [SerializeField] ScriptableObj IdentityObj = null;
    [SerializeField] ScriptableObj HorrorEvnetsObj = null;
    [SerializeField] ScriptableObj PuzzleObj = null;

    private Dictionary<string, AudioClip> PlayerClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> InteractableClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> EnviromentClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> IdentiyuClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> HorrorEvnetsClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> PuzzleClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        AddAllClipsToDic(PlayerObj);
        AddAllClipsToDic(InteractableObj);
        AddAllClipsToDic(EnviromentObj);
        AddAllClipsToDic(IdentityObj);
        AddAllClipsToDic(HorrorEvnetsObj);
        AddAllClipsToDic(PuzzleObj);
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

            case "Identity":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    IdentiyuClips.Add(IdentityObj.Sounds[i].name, IdentityObj.Sounds[i]);
                }
                break;
            case "HorrorEventSounds":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    HorrorEvnetsClips.Add(HorrorEvnetsObj.Sounds[i].name, HorrorEvnetsObj.Sounds[i]);
                }
                break;
            case "Puzzle":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    PuzzleClips.Add(PuzzleObj.Sounds[i].name, PuzzleObj.Sounds[i]);
                }
                break;
        }
    }

    /// <summary>
    /// return requested Audioclip for others
    /// </summary>
    /// <param name="type">The type in AudioManager containing the audio clip to request.</param>
    /// <param name="clipName">The name of requested audioclip</param>
    /// <returns></returns>
    public AudioClip GetClip(Type type, string clipName)
    {
        return CheckType(type).ContainsKey(clipName) ? CheckType(type)[clipName] : null;
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

            case Type.HorrorEvnets:
                curdic = HorrorEvnetsClips;
                break;

            case Type.Interactable:
                curdic = InteractableClips;
                break;

            case Type.Environment:
                curdic = EnviromentClips;
                break;

            case Type.Puzzle:
                curdic = PuzzleClips;
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
