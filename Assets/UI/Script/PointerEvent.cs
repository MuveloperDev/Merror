using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    Image[] _backgroundImage;

    private void Start()
    {
        
    }

    private void Awake()
    {
        _backgroundImage = GetComponentsInChildren<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _backgroundImage[1].enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _backgroundImage[1].enabled = false;
    }
}
