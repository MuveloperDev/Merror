using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using UnityEngine.UI;
using TMPro;
using EPOOutline;

public class FreemasonCipher : MonoBehaviour
{
    [SerializeField] GameObject BlackBoardBackground;
    [SerializeField] Button CloseButton;

    private void Awake()
    {
        //BlackBoardBackground.enabled = true;
        //BlackBoardBackground.sortingOrder = 1;
    }

    private void inputBoard()
    {
        Debug.Log("inputBoard");
        TimeControl.Pause();
        BlackBoardBackground.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ClickCloseButton()
    {
        BlackBoardBackground.SetActive(false);
        TimeControl.Play();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
