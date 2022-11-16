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
    [SerializeField] TextMeshProUGUI inputStr = null;
    [SerializeField] GameObject BlackBoard = null;
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

    public void OnValueChange()
    {
        string input = inputStr.text;

        char[] convertCharArray = input.ToCharArray();

        if (input != null)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                int toInt = (int)input[i];
                if (toInt > 96)
                {
                    convertCharArray[i] = (char)(toInt - 32);
                }

                else if (toInt < 96)
                {
                    convertCharArray[i] = (char)toInt;
                }
            }
            input = new string(convertCharArray).Remove(convertCharArray.Length - 1);
        }

        Debug.Log(input);

        // 분리 필요
        if (input == answerString)
        {
            Debug.Log("퍼즐 풀이 성공");
            blackBoard.CallChangeBlackBoardAlpha();
            DropHint();
            //gameObject.SendMessage("CallChangeBlackBoardAlpha", SendMessageOptions.DontRequireReceiver);
            DeleteOutline();
            SolvedPuzzle();
            ClickCloseButton();
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

            blackBoard.CallChangeBlackBoardAlpha();
            DeleteOutline();
            SolvedPuzzle();
            ClickCloseButton();
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
        Destroy(BlackBoard.GetComponent<Outlinable>());
        Destroy(BlackBoard.GetComponent<Interactable>());
    }

    private void DropHint()
    {
        if (isDrop == false)
        {
            Debug.Log("DropHint");
            GameManager.Instance.ClearPuzzle("FreemasonCipher", Hint2, 7f);
            isDrop = true;
        }
    }
}

