using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzleCanvasManagement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI inputStr;
    [SerializeField] FreemasonCipher freemasonCipher;


    // ¿€º∫¡ﬂ
    public void OnValueChange()
    {
        string input = inputStr.text;

        char[] convertCharArray = input.ToCharArray();

        if (input != null)
        {
            for (int i = 0; i < 11; i++)
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
            freemasonCipher.CompareAnswer(input);
        }
    }
}
