using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;


public class PuzzleGuessWho : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private GameObject hintObj = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private PuzzleGuessWho[] Objs = null;
    [SerializeField] private MeshRenderer draw = null;
    [SerializeField] private Material changingDraw = null;
    [SerializeField] private bool isClear = false;
    [SerializeField] private bool isCorrect = false;
    [SerializeField] private Interactable doll = null;
    [SerializeField] private BoxCollider endTrigger = null;

    private void OnEnable() => Init();

    void Init()
    {
        Objs = GameObject.FindObjectsOfType<PuzzleGuessWho>();
        player = GameObject.FindObjectOfType<Player>();
    }


    void CheckAnswer()
    {
        if (isCorrect)
        {
            DropKey();
            isClear = true;
        }
        else player.SendMessage("Death", CameraState.CamState.DEATH, SendMessageOptions.DontRequireReceiver);

        if (isClear) foreach (var obj in Objs) Destroy(obj.GetComponent<Interactable>());
    }

    void DropKey()
    {
        audioSource.clip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Identity, "Isabel_Gigle");
        audioSource.Play();
        draw.material = changingDraw;
        InsertItemInventory();
        GameManager.Instance.GetIdentityManager().OnEnableIdentity(new Vector3(41f, 8.5f, 14f), Quaternion.Euler(0, 53f, 0), BaseStateMachine.State.SITTINGANDFOCUS);
        doll.gameObject.SetActive(true);
        endTrigger.gameObject.SetActive(true);

        Invoke(nameof(AudioourceDisable), 2f);
    }

    void AudioourceDisable() => audioSource.gameObject.SetActive(false);
    private void InsertItemInventory() => GameManager.Instance.ClearPuzzle(nameof(PuzzleGuessWho), hintObj, 7f);
    public void MySpecial() => CheckAnswer();

}
