using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGuessWho : MonoBehaviour
{
    [SerializeField] private Player player;

    private void OnEnable() => Init();

    public void Init() => player = GameObject.FindObjectOfType<Player>();

    
    void CheckAnswer()
    {
        if (gameObject.name == "Isabel") DropKey();
        else player.SendMessage("Death", CameraState.CamState.PANIC, SendMessageOptions.DontRequireReceiver);
    }

    void DropKey() => Debug.Log("Drop key by BK ObjectsPool");

}
