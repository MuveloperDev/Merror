using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    GameObject soundManager = null;
    
    Button[] transButton;

    GameObject button;

    private void Awake()
    {
        soundManager = GameObject.Find("SoundOption");
        transButton = GameObject.Find("Menu").GetComponentsInChildren<Button>();
    }


    // NewGameButton
    public void OnClickNewGame()
    {
        Debug.Log("게임 시작");
        // LodingScene AsyncLoad
        SceneManager.LoadSceneAsync("LodingScene");
    }

    // OptionButton
    public void OnClickOption()
    {
        Debug.Log("옵션");
        // Activate SoundManager UI
        soundManager.SetActive(true);
    }

    // SaveButton
    public void OnClickSave()
    {
        Debug.Log("불러오기");
    }

    // ExitButton
    public void OnClickExit()
    {
        // Works only in UnityEditor
        UnityEditor.EditorApplication.isPlaying = false;

        // Works only in Applications
        // Application.Quit();
    }

    // SoundButton Close
    public void OnClickClose()
    {
        soundManager.SetActive(false);
    }
}
