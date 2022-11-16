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

    [SerializeField] GameObject BlackBoardBackground = null;
    //[SerializeField] Button CloseButton;
    [SerializeField] GameObject[] HintPaper = null;
    [SerializeField] BlackBoard blackBoard = null;

    [SerializeField] GameObject Hint2 = null;

    private AudioSource playChalkSound = null;

    private const string answerString = "DESTROYDOLL";

    private bool isDrop = false;

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

    // 추가 중
    public void CompareAnswer(string inStr)
    {

        if (inStr == null)
        {
            return;
        }

        else if (inStr == answerString)
        {
            Debug.Log("퍼즐 정답");

            InsertItemInventory();
            GameManager.Instance.Save();
            
        }
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

    private void DeleteOutline()
    {
        Destroy(this.gameObject.GetComponent<Outlinable>());
        Destroy(this.gameObject.GetComponent<Interactable>());
    }

    private void InsertItemInventory()
    {
        if (isDrop == false)
        {
            Debug.Log("DropHint");

            GameManager.Instance.ClearPuzzle("FreemasonCipher", Hint2, 7f);
            isDrop = true;
        }
        blackBoard.CallChangeBlackBoardAlpha();
        DeleteOutline();
        SolvedPuzzle();
        ClickCloseButton();
    }
}

