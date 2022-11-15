using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;

public class PuzzleGuessWho : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private GameObject hintObj = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private PuzzleGuessWho[] Objs = null;
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
        audioSource.clip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Puzzle, "GuessWho_Scream");
        audioSource.Play();

        GameManager.Instance.ClearPuzzle(hintObj,7f);
        GameManager.Instance.GetIdentityManager().OnEnableIdentity(new Vector3(35f, 9f, 14f), Quaternion.Euler(0, -120f, 0),BaseStateMachine.State.SLEEPING);

        
        Invoke("AudioourceDisable", 2f);
    }

    void AudioourceDisable() => audioSource.gameObject.SetActive(false);
}
