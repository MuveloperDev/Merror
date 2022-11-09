using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class IntroFade : MonoBehaviour
{
    // Panel Image
    Image panelImage = null;
    

    private void Awake()
    {
        panelImage = gameObject.GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PanelAlpha());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Increase in alpha value
    IEnumerator PanelAlpha()
    {
        float fadeTime = 1;

        while (fadeTime > 0f)
        {
            yield return new WaitForSeconds(0.1f);

            fadeTime -= 0.015f;
            panelImage.color = new Color(0, 0, 0, fadeTime);
            Debug.Log("fadeTime" + fadeTime);
        }
        SceneManager.LoadSceneAsync("StartScene");
    }
}
