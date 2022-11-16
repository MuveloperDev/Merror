using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using Newtonsoft.Json.Linq;

public class PuzzleGuessWho : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private GameObject hintObj = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private PuzzleGuessWho[] Objs = null;
    [SerializeField] private MeshRenderer draw = null;
    [SerializeField] private Material changingDraw= null;
    [SerializeField] private bool isClear = false;

    private void OnEnable() => Init();

    void Init()
    {
        Objs = GameObject.FindObjectsOfType<PuzzleGuessWho>();
        player = GameObject.FindObjectOfType<Player>();
    }

    
    void CheckAnswer()
    {
        if (gameObject.name == "Isabel")
        {
            DropKey();
            isClear = true;
        }
        else player.SendMessage("Death", CameraState.CamState.DEATH, SendMessageOptions.DontRequireReceiver);

        if (isClear) foreach (var obj in Objs) Destroy(obj.GetComponent<Interactable>());
    }

    void DropKey()
    { 
        Debug.Log("Drop key by BK ObjectsPool");
        audioSource.clip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Identity, "Isabel_Gigle");
        audioSource.Play();
        draw.material = changingDraw;
        GameManager.Instance.ClearPuzzle("PuzzleGuessWho", hintObj,7f);
        GameManager.Instance.GetIdentityManager().OnEnableIdentity(new Vector3(41f, 8.5f, 14f), Quaternion.Euler(0, 53f, 0),BaseStateMachine.State.SITTINGANDFOCUS);

        
        Invoke("AudioourceDisable", 2f);
    }

    void AudioourceDisable() => audioSource.gameObject.SetActive(false);
}
