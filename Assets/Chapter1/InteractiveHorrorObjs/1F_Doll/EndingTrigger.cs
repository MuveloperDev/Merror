using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingTrigger : MonoBehaviour
{
    [SerializeField] private GameObject isabelDoll = null;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Inven : " + GameManager.Instance.GetInventory().FindInInven(isabelDoll));
        if (other.CompareTag("Player") && GameManager.Instance.GetInventory().FindInInven(isabelDoll))
        {
            gameObject.SetActive(false);
            Debug.Log("Inven : " + GameManager.Instance.GetInventory().FindInInven(isabelDoll));
            GameManager.Instance.GetVideoPlayer().CallPlayVideo(
                GameManager.Instance.GetVideoPlayer().getVideoClips.getChapter1.EndVideo,
                () => {
                    GameManager.Instance.GetVideoPlayer().CallPlayVideo(
                GameManager.Instance.GetVideoPlayer().getVideoClips.getChapter1.DemoEndVideo,
                () => { SceneManager.LoadScene("StartScene"); }
                );
                }
                );
        }

    }
}
