using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MyLibrary;
using EPOOutline;

public class Decryption_Puzzle : MonoBehaviour
{
    [SerializeField] private GameObject DecryptionUI = null;
    [SerializeField] private TextMeshProUGUI resultTXT = null;
    [SerializeField] private GameObject ClearHint = null;

    static private bool isComplete = false;
    private const string puzzleAnswer = "SEVEN";

    private void VisibleUI()
    {
        TimeControl.Pause();
        GameManager.Instance.ToggleCursor(false);
        DecryptionUI.gameObject.SetActive(true);
    }

    private void DisVisibleUI()
    {
        TimeControl.Play();
        resultTXT.text = "";
        GameManager.Instance.ToggleCursor(true);
        DecryptionUI.gameObject.SetActive(false);
    }

    public void OnInputEnter(string value)
    {
        if (value.Length <= 0)
            return;

        if (value[value.Length - 1].Equals('\n'))
            DecryotionText(value);
    }

    public void OnClickCloseBtn()
    {
        DisVisibleUI();
    }

    // 65 ~ 90 A ~ Z
    private void DecryotionText(string value)
    {
        string inputString = value.ToUpper();

        char[] charArr = inputString.ToCharArray();

        for (int i = 0; i < charArr.Length - 1; i++)
        {
            int convertInt = (int)charArr[i];
            if (convertInt < 65 || convertInt > 90)
                continue;

            convertInt -= 5;

            if (convertInt < 65)
            {
                convertInt = 90 - (64 - convertInt);
            }

            charArr[i] = (char)convertInt;
        }

        inputString = new string(charArr).Remove(charArr.Length - 1);
        if(inputString.Equals(puzzleAnswer) == true)
        {
            CompletePuzzle();
        }
        else
        {
            resultTXT.text = "RESULT \n" + inputString + "\nWrong PassWord";
        }
    }
    
    private void CompletePuzzle()
    {
        isComplete = true;
        Interactable interactable = GetComponent<Interactable>();
        interactable.SetSpecial(false);
        interactable.NonInteractable();
        InsertItemInventory();
        DisVisibleUI();
        GameManager.Instance.Save();
    }

    private void InsertItemInventory()
    {
        GameManager.Instance.ClearPuzzle("Decryption_Puzzle", ClearHint, ClearHint.GetComponent<Interactable>().GetInvenScale());
    }
}
