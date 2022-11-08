using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MyLibrary;

public class Decryption_Puzzle : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultTXT = null;

    void Start()
    {
        StartCoroutine(TEST());
    }

    IEnumerator TEST()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Pause");
        TimeControl.Pause();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnInputEnter(string value)
    {
        if (value[value.Length - 1].Equals('\n'))
            DecryotionText();
    }

    // 65 ~ 90 A ~ Z
    private void DecryotionText()
    {
        string inputString = resultTXT.text.ToUpper();

        char[] charArr = inputString.ToCharArray();

        for (int i = 0; i < charArr.Length - 1; i++)
        {
            int convertInt = (int)charArr[i];
            //if (convertInt < 65 || convertInt > 90)
            //    continue;

            convertInt -= 5;

            if (convertInt < 65)
            {
                convertInt = 90 - (64 - convertInt);
            }

            charArr[i] = (char)convertInt;

        }
        inputString = new string(charArr);
        resultTXT.text = "RESULT \n" + inputString;
    }
    
}
