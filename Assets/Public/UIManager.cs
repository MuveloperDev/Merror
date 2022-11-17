using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyLibrary;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;
using Unity.VisualScripting;
using TMPro;
using System.IO;
using System.Reflection.Emit;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject aim;
    [SerializeField] GameObject mainUI;

    Button[] transButtons;

    // Button Children image gameobject
    [SerializeField] Image buttonBackgroundImage;

    // LoadGame Children image gameobject
    [SerializeField] Image LoadButtonImage;

    [SerializeField] TextMeshProUGUI transLanguage = null;



    GameObject soundManager = null;

    private UIManager() { }
    #region Player Stamina UI
    [SerializeField] private Slider Stamina = null;

    public void UpdateStamina(float changed)
    {
        Stamina.value = changed;
    }
    #endregion

    private void Awake()
    {
        soundManager = GameObject.Find("Option");
        transButtons = GameObject.Find("Menu").GetComponentsInChildren<Button>();

        // Move UI to front
        aim.transform.SetAsLastSibling();
        DontDestroyOnLoad(aim);
    }

    private void Start()
    {
        aim.SetActive(false);
        FileInfo SaveFileInfo = new FileInfo(System.IO.Directory.GetCurrentDirectory() + "/SaveData/Data.txt");
        if (SaveFileInfo.Exists == false)
        {
            GameObject loadBtn = GameObject.Find("LoadButton");
            loadBtn.GetComponent<Button>().interactable = false;
            Destroy(loadBtn.GetComponent<PointerEvent>());
        }
        // Load Game Data�� ����� ��츦 ��Ÿ�״� bool ���� �ʿ�
        //Load Data : Null => Button interactable : false
        //if (transButtons[1] == null)
        //{
        //   Destroy(LoadButtonImage);
        //   transButtons[1].interactable = false;    
    }

    //#region Button Event Trigger
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    Debug.Log("OnPointerEnter");
    //    Debug.Log(gameObject.name);
    //}
    //
    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    Debug.Log("OnPointerExit");
    //}
    //#endregion // StartScene Envent Trigger

    #region Button Click Event
    // NewGameButton
    public void OnClickNewGame()
    {
        GameManager.Instance.GetCaptureManager().Init();
        // LodingScene AsyncLoad
#if UNITY_EDITOR
        SceneManager.LoadSceneAsync("LodingScene");
        return;
#else
        SceneManager.LoadScene("DaytimeScene");
#endif
    }

    // OptionButton 
    public void OnClickOption()
    {
        // Activate SoundManager UI
        soundManager.SetActive(true);
    }

    // SaveButton
    public void OnClickLoadGame()
    {
        GameManager.Instance.GetCaptureManager().Init();
        GameManager.Instance.Load();
        SceneManager.LoadSceneAsync("LodingScene");
        Debug.LogError("Load Game");
    }

    // ExitButton
    public void OnClickExit()
    {
#if UNITY_EDITOR
        // Works only in UnityEditor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        // Works only in Applications
        // Application.Quit();
    }

    // SoundButton Close
    public void OnClickClose()
    {
        soundManager.SetActive(false);
    }

#endregion

    #region SelectLanguage
    public void SelectLanguage()
    {
        string inStr = transLanguage.text;
        switch (inStr)
        {
            case "English":
                Debug.Log("English");

                GameManager.Instance.GetCaptureManager().Language = CaptureManager.LanguageCategory.ENGLISH;
                break;
            case "Korean":
                Debug.Log("Korean");
                GameManager.Instance.GetCaptureManager().Language = CaptureManager.LanguageCategory.KOREAN;
                break;
        }
    }


    #endregion

    // Raycast �и� ���� 
    // *** ray -> Player : interactorable �Ǵ� 
    // O : mytype -> ���콺 Ŀ�� ����
    // X : default ���콺 Ŀ���� ����
    //---------------------------------------------------------------------
    // Contact Mylibrary => interactable Script

    /// <summary>
    /// When player mouse ray enter or exit the object.
    /// Change mouse aim 
    /// </summary>
    /// <param name="value">Outline enabled state</param>
    public void ChangeAimIcon(string myType, bool value)
    {
        if (gameObject.scene.name == "Chapter1")
        {
            aim.SetActive(!value);
        }
    }

    /*
        public virtual void Do_Outline(bool value)
        {
            if (_Outlinable != null)
                _Outlinable.enabled = value;
            GameManager.Instance.GetUI().ChangeIcon(myType, value);
        }
    */
    [SerializeField] Slider acquisitionNotificationSlider = null;
    [SerializeField] TextMeshProUGUI acquisitionNotificationText = null;
    [SerializeField] AudioSource uiAudioSource = null;

    public void InitAcquisitionNotification()
    {
        acquisitionNotificationSlider = GameObject.Find("AcquisitionNotification").GetComponent<Slider>();
        acquisitionNotificationText = GameObject.Find("AcquisitionNotificationText").GetComponent<TextMeshProUGUI>();
        uiAudioSource = acquisitionNotificationSlider.GetComponent<AudioSource>();
        acquisitionNotificationText.gameObject.SetActive(false);
    }
    public void AcquisitionNotification(string name)
    {
        if (GameManager.Instance.completeLoad == false)
            return;

        acquisitionNotificationText.text = "Added Inventory The " + name;
        if (uiAudioSource.clip == null)
            uiAudioSource.clip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Interactable, "UI_AddInventory");
        uiAudioSource.PlayOneShot(uiAudioSource.clip);
        StartCoroutine(StartFill(acquisitionNotificationSlider, acquisitionNotificationText));
    }

    IEnumerator StartFill(Slider slider, TextMeshProUGUI text)
    {
        float fillValue = 0f;
        while (slider.value != 1)
        {
            fillValue += Time.deltaTime * 7f;
            slider.value = fillValue;
            yield return new WaitForFixedUpdate();
        }
        text.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        text.gameObject.SetActive(false);
        while (slider.value != 0f)
        {
            fillValue -= Time.deltaTime * 7f;
            slider.value = fillValue;
            yield return new WaitForFixedUpdate();
        }
    }
}
