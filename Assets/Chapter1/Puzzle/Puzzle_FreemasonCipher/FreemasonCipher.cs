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
    [SerializeField] GameObject[] HintPaper;
    [SerializeField] TextMeshProUGUI inputStr;

    private string answerString = "DESTROYDOLL";

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

    public void SolvedPuzzle()
    {
        foreach (GameObject hint in HintPaper)
        {
            hint.SetActive(false);
        }
    }

    public void OnValueChange()
    {
        inputStr.text.ToUpper();

        Debug.Log(inputStr.text);

        if (answerString.CompareTo(inputStr.text) == 0)
        {
            ClickCloseButton();
        }
    }
}
