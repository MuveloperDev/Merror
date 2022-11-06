using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMaterial : MonoBehaviour
{
    public Material[] UIMaterial;
    private Image AimUI;
    RaycastHit RaycastHit;

    private void Awake()
    {
        //Component initialization
        AimUI = GetComponent<Image>();

        // Load material from Resource/UIMaterial folder
        UIMaterial = Resources.LoadAll<Material>("UIMaterial");
    }

    // Start is called before the first frame update
    void Start()
    {
        // Default aim setting
        AimUI.material = UIMaterial[5];
    }

    // Update is called once per frame
    void Update()
    {
        //ChangeAim();
    }

    // Change Aim according to tag
    /*private void ChangeAim()
    {
        switch (RaycastHit)
        {
            case door:
                AimUI.material = UIMaterial[1];
                break;
        }
    }*/
}
