using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyLibrary;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
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
        
    }


    // Add Pointer Script
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
