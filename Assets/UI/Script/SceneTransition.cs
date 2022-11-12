using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] Image Logo;
    public AsyncOperation loadAO;

    bool isloading = false;

    private void Awake()
    {
        // Play ingame Scene 
        // SceneManager.LoadScene("Chapter1");
        loadAO = SceneManager.LoadSceneAsync("Chapter1", LoadSceneMode.Additive);
        Logo.fillAmount = 0;
    }

    //private void Start()
    //{
    //    //StartCoroutine(AllowScene());
    //}

    private void Update()
    {
        Logo.fillAmount = loadAO.progress;


        if (loadAO.isDone == true)
        {
            SceneManager.UnloadScene("LodingScene");
            loadAO.allowSceneActivation = true;
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
