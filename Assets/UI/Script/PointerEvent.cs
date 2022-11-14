using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image[] buttonBackgroundImage;

    private void Awake()
    {
        buttonBackgroundImage = GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        // get Children image
        buttonBackgroundImage[1].fillAmount = 0f;
        buttonBackgroundImage[1].enabled = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonBackgroundImage[1].enabled = true;
        StartCoroutine(ChangeFilled());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // fillamount reset
        buttonBackgroundImage[1].fillAmount = 0f;
        buttonBackgroundImage[1].enabled = false;
    }

    // change Image FillAmout to 0~1
    IEnumerator ChangeFilled()
    {
        float fillSpeed = 0.05f;

        while (buttonBackgroundImage[1].fillAmount < 1)
        {
            yield return new WaitForSeconds(0.01f);
            buttonBackgroundImage[1].fillAmount += fillSpeed;
        }
        yield return null;
    }
}
