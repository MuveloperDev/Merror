using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    [SerializeField] private GameObject isabelDoll = null;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Inven : " + GameManager.Instance.GetInventory().FindInInven(isabelDoll));
        if (other.CompareTag("Player") && GameManager.Instance.GetInventory().FindInInven(isabelDoll))
        {
            Debug.Log("Inven : " + GameManager.Instance.GetInventory().FindInInven(isabelDoll));
            GameManager.Instance.GetVideoPlayer().CallPlayVideo(
                GameManager.Instance.GetVideoPlayer().getVideoClips.getChapter1.EndVideo,
                () => { Debug.Log("CallLoadScene ThankYou"); }
                );
        }

    }
}
