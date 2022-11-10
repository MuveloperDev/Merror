using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyLibrary;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject aim;

    Button[] transButtons;

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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        aim.SetActive(false);
    }

    // Add Pointer Script

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
        Debug.LogError("Load Game");
        //transButtons[1]
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
