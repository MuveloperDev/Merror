using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using UnityEngine.UI;
using TMPro;
using EPOOutline;

public class FreemasonCipher : MonoBehaviour
{
    [SerializeField] AudioClip[] ChalkSound = null;
    [SerializeField] GameObject BlackBoardBackground;
    [SerializeField] Button CloseButton;
    [SerializeField] GameObject[] HintPaper;
    [SerializeField] TextMeshProUGUI inputStr;

    private AudioSource playChalkSound = null;

    private string answerString = "DESTROYDOLL";

    private void Awake()
    {
        playChalkSound = GetComponent<AudioSource>();

        ChalkSound = new AudioClip[5]
        {
        GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Interactable, "ChalkSound1"),
        GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Interactable, "ChalkSound2"),
        GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Interactable, "ChalkSound3"),
        GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Interactable, "ChalkSound4"),
        GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Interactable, "ChalkSound5")
        };
        //BlackBoardBackground.enabled = true;
        //BlackBoardBackground.sortingOrder = 1;

        Debug.Log(ChalkSound[0].name);
        Debug.Log(ChalkSound[1].name);
        Debug.Log(ChalkSound[2].name);
        Debug.Log(ChalkSound[3].name);
        Debug.Log(ChalkSound[4].name);
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
        if (HintPaper != null)
        {
            for (int i = 0; i < HintPaper.Length; i++)
            {
                HintPaper[i].SetActive(false);
            }
        }
    }

    public void OnValueChange()
    {
        string input = inputStr.text.ToUpper();

        for (int i = 0; i < input.Length - 1; i++)
        {
            if (input[i] == answerString[i]) continue;
            else return;
        }
        SolvedPuzzle();
        ClickCloseButton();
    }

    public void PalyChalkSound()
    {
        if (ChalkSound != null)
        {
            int playRandomClip = Random.Range(0, 4);
            playChalkSound.clip = ChalkSound[playRandomClip];
            playChalkSound.Play();
        }
    }
}

