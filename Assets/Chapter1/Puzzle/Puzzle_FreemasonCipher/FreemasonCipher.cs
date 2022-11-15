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
    //[SerializeField] Button CloseButton;
    [SerializeField] GameObject[] HintPaper;
    [SerializeField] TextMeshProUGUI inputStr;
    [SerializeField] GameObject BlackBoard;

    private AudioSource playChalkSound = null;

    private const string answerString = "DESTROYDOLL";

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


        if (input == answerString)
        {
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

    IEnumerator DestroyInteractable()
    {
        BlackBoard.TryGetComponent<Renderer>(Material);
        yield break;
    }
}

