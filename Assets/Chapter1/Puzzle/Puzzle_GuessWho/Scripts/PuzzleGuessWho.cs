using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;

public class PuzzleGuessWho : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private GameObject hintObj = null;

    private void OnEnable() => Init();

    public void Init()
    {
        player = GameObject.FindObjectOfType<Player>();
        hintObj.SetActive(false);
    }

    
    void CheckAnswer()
    {
        if (gameObject.name == "Isabel") DropKey();
        else player.SendMessage("Death", CameraState.CamState.PANIC, SendMessageOptions.DontRequireReceiver);
    }

    void DropKey()
    { 
        Debug.Log("Drop key by BK ObjectsPool");
        hintObj.SetActive(true);
    }

}
