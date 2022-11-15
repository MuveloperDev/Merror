using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] Image Logo;
    public AsyncOperation loadAO;

    float fillAmountBar = 0.01f;
    float timer = 0.01f;
    bool isloading = false;
    private delegate IEnumerator Del_FillLoadingBar(int ChapterNum);
    private Del_FillLoadingBar fillLoading;

    private void Awake()
    {
        fillLoading = FillLoadingBar;
        if (fillLoading != null)
            StartCoroutine(fillLoading(GameManager.Instance.GetData().chapter));
        else
            Debug.LogError("Can not found FillLoading Coroutine");
        // Play ingame Scene 
        // SceneManager.LoadScene("Chapter1");

        //loadAO = SceneManager.LoadSceneAsync("Chapter1_SSH", LoadSceneMode.Additive);
        Logo.fillAmount = 0;
    }


    private void Update()
    {
        if (loadAO == null)
            return;

        if (loadAO.isDone == true)
        {
            SceneManager.UnloadSceneAsync("LodingScene");
            loadAO.allowSceneActivation = true;
        }
    }

    IEnumerator FillLoadingBar(int ChapterNum)
    {
        string chapter = null;
        switch (ChapterNum)
        {
            case 1:
                chapter = "Chapter1";
                break;
            case 2:
                chapter = "Chapter2";
                break;
            case 3:
                chapter = "Chapter3";
                break;
            case 4:
                chapter = "Chapter4";
                break;
            case 5:
                chapter = "Chapter5";
                break;
            default:
                Debug.LogError("Chapter Number Error");
                break;
        }

        if (chapter == null)
        {
            Debug.LogError("Chapter string is null");
            yield break;
        }

        loadAO = SceneManager.LoadSceneAsync(chapter, LoadSceneMode.Additive);


        yield return new WaitForFixedUpdate();

        while (!loadAO.isDone)
        {
            yield return new WaitForSeconds(0.02f);

            timer += fillAmountBar;

            if (loadAO.progress <= 0.9)
            {

                if (timer < 0.5f)
                {
                    Logo.fillAmount += fillAmountBar;
                }

                else if (timer >= 0.5f)
                {
                    Logo.fillAmount = 0.9f;
                }
            }
            else
            {
                Debug.Log("�ε� �Ϸ�");
            }
        }
    }
}