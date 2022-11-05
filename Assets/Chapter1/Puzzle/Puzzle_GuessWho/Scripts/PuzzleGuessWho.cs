using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGuessWho : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private string name;


    private void OnEnable() => Init();

    public void Init()
    { 
        name = gameObject.name;
        player = GameObject.FindObjectOfType<Player>();
    } 
    
    void CheckAnswer()
    {
        Debug.Log("Check");
        if (name == "Isabel") DropKey();
        else player.SendMessage("Death", CameraState.CamState.PANIC, SendMessageOptions.DontRequireReceiver);
    }

    void DropKey() => Debug.Log("Drop key by BK ObjectsPool");

}
