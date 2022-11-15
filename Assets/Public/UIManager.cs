using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyLibrary;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject aim;
    [SerializeField] GameObject mainUI;

    Button[] transButtons;

    // Button Children image gameobject
    [SerializeField] Image buttonBackgroundImage;

    // LoadGame Children image gameobject
    [SerializeField] Image LoadButtonImage;

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
        soundManager = GameObject.Find("SoundOption");
        transButtons = GameObject.Find("Menu").GetComponentsInChildren<Button>();

        // Move UI to front
        aim.transform.SetAsLastSibling();
        DontDestroyOnLoad(aim);
    }

    private void Start()
    {
        aim.SetActive(false);

        // Load Game Data가 비었을 경우를 나타네는 bool 변수 필요
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
        // LodingScene AsyncLoad
        SceneManager.LoadSceneAsync("LodingScene");
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








    // Raycast 분리 이후 
    // *** ray -> Player : interactorable 판단 
    // O : mytype -> 마우스 커서 변경
    // X : default 마우스 커서로 변경
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
}
