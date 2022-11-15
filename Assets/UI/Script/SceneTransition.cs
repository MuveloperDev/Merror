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

    private void Awake()
    {
        // Play ingame Scene 
        // SceneManager.LoadScene("Chapter1");
        //loadAO = SceneManager.LoadSceneAsync("Chapter1", LoadSceneMode.Additive);
        loadAO = SceneManager.LoadSceneAsync("Chapter1_SSH", LoadSceneMode.Additive);
        Logo.fillAmount = 0;
    }

    private void Start()
    {
        StartCoroutine(FillLoadingBar());
    }


    private void Update()
    {
        if (loadAO.isDone == true)
        {
            SceneManager.UnloadSceneAsync("LodingScene");
            loadAO.allowSceneActivation = true;
        }
    }

    IEnumerator FillLoadingBar()
    {
        yield return new WaitForFixedUpdate();

        while (!loadAO.isDone)
        {
            yield return new WaitForSeconds(0.02f);
            Debug.Log("while문 들어옴");
            Debug.Log(loadAO.progress);

            timer += fillAmountBar;

            if (loadAO.progress <= 0.9)
            {
                Debug.Log("if문 도는 중"+ Logo.fillAmount);

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
                Debug.Log("로딩 완료");
            }
        }
    }



    //IEnumerator AllowScene()
    //{
    //    Debug.Log("AllowScene");
    //    while (loadAO.isDone == true)
    //    {
    //        if (loadAO.progress == 1)
    //        {
    //            Debug.Log(loadAO.isDone);
    //            // Start Chapter1 Scene when ready
    //            SceneManager.UnloadScene("LodingScene");
    //            loadAO.allowSceneActivation = true;
    //        }
    //    }
    //    yield return null;
    //}
}
