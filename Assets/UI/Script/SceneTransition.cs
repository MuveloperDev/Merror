using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private void Awake()
    {
        // Play ingame Scene 
        SceneManager.LoadScene("Chapter1");
    }

}
