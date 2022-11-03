using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;


public class CaptionManager : MonoBehaviour
{
    /// <summary>
    /// Defined language
    /// </summary>
    public enum Language
    {
        ENGLISH,
        KOREAN,   // Test
    }

    private Language language = Language.KOREAN;

    /// <summary>
    /// Defined Captions
    /// </summary>
    public enum Captions
    {
        DATE,
        REMEMBER,
    }

    [Header("Chapter Info")]
    [SerializeField] private int chapter;

    [Header("Caption UI")]
    [SerializeField] private CanvasGroup ui_capturePanel = null;
    [SerializeField] private TextMeshProUGUI ui_captionTxt = null;

    [Header("Caption PathList")]
    [SerializeField] List<string> pathsList = new List<string>();
    [SerializeField] List<string> fileNameList = new List<string>();

    // Folder information of the top-most path.
    private DirectoryInfo textFilesInfo = null;  

    // Language-specific folder path.
    private Dictionary<int, string> languageFilePath = new Dictionary<int, string>();

    // Combine captions as a list and store them in the caption table.
    private Dictionary<string , List<string>> captionTable = new Dictionary<string , List<string>>();

    // Reads a file in the specified folder.
    private DirectoryInfo directoryInfo = null;




    #region Property
    public int Chapter { get { return chapter; } }      // Current chapter
    #endregion


    private void Start() => Init();

    private void Init()
    {
        ReadCaption(language);

        ui_captionTxt.text = "";
        chapter = 0;
        ui_capturePanel.alpha = 0f;
    }

    /// <summary>
    /// All text files under the selected language folder are read and saved in pathList.
    /// </summary>
    /// <param name="language">Defined language by enum</param>
    private void ReadCaption(Language language)
    {

        textFilesInfo = new DirectoryInfo("C:\\Users\\ich96\\UnityProjects\\root_Merror\\MerrorProject\\Assets\\TextFiles");

        for (int i = 0; i < textFilesInfo.GetDirectories().Length; i++)
            languageFilePath.Add(i, textFilesInfo.GetDirectories()[i].FullName);

        // 선택된 경로 추가.
        directoryInfo = new DirectoryInfo(languageFilePath[(int)language]);

        // 디렉토리에서 파일 경로 가져온 후 경로리스트에 추가.
        foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
        {
            if (fileInfo.Extension == ".meta") continue;
            pathsList.Add(fileInfo.FullName);
            fileNameList.Add(fileInfo.Name.Replace(".txt",""));
        }

        // // Saved CationList After Read CationTextFile From PathsList.
        for (int i = 0; i < pathsList.Count; i++)
        {
            List<string> captionsList = new List<string>();
            using (FileStream fileStream = new FileStream(pathsList[i], FileMode.Open))
            {
                StreamReader reader = new StreamReader(fileStream);
                while (!reader.EndOfStream) captionsList.Add(reader.ReadLine());
            }
            captionTable.Add(fileNameList[i], captionsList);
        }
    }

    // Set text in caption list after Increase caption UI alpha.
    public void TurnOnCaption(Captions caption, int captionIdx)
    {
        if (ui_captionTxt.text.Length != 0) ui_captionTxt.text = "";
        StartCoroutine(SetCaptionPanelAlpha(1, captionTable[caption.ToString()][captionIdx]));
        chapter++;
    }

    // Set text in caption list after Decrease caption UI alpha.
    public void TurnOffCaption() => StartCoroutine(SetCaptionPanelAlpha(0, ""));


    /// <summary>
    /// Set caption alpha and assign caption text.
    /// </summary>
    /// <param name="alpha">Set the alpha value. Assign only 0 or 1</param>
    /// <param name="caption">Assgin value for caption text</param>
    /// <returns></returns>
    IEnumerator SetCaptionPanelAlpha(int alpha, string caption)
    {
        float value = alpha == 1 ? Time.deltaTime * 6f : -Time.deltaTime * 3f;
        do
        {
            yield return null;
            ui_capturePanel.alpha += value;
        } while (ui_capturePanel.alpha != alpha);
        ui_captionTxt.text = caption;
    }
}
