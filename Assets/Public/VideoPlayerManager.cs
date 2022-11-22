using MyLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class CustomDictionary<T1, T2> where T1 : class where T2 : class
{
    [SerializeField] private T1[] keys;
    public T1[] Keys { get { return keys; } }
    [SerializeField] private T2[] values;
    public T2[] Values { get { return values; } }

    [SerializeField] private Dictionary<T1, T2> dictionary = new Dictionary<T1, T2>();
    public void Init()
    {
        for (int i = 0; i < keys.Length; i++) dictionary.Add(keys[i], values[i]);
    }

    public T2 GetValue(T1 key) => dictionary[key];
}

public class VideoPlayerManager : MonoBehaviour
{
    public enum VideoCategory
    {
        PUBLIC,
        CHAPTER1,
    }
    [SerializeField] private CustomDictionary<string, VideoClip> clips = new CustomDictionary<string, VideoClip>();

    [SerializeField] private VideoPlayer videoPlayer = null;

    private void Start() => Init();
    void Init()
    { 
        videoPlayer = GetComponent<VideoPlayer>();
        clips.Init();
    } 


    /// <summary>
    /// Play the video referenced in VideoClips on the main camera.
    /// </summary>
    /// <param name="clip">This is the video to be played.</param>
    /// <param name="func">Callback function to be executed at the end of the video.</param>
    public void CallPlayVideo(VideoClip clip, Action func) => StartCoroutine(PlayVideo(clip, func));
    /// <summary>
    /// Play the video referenced in VideoClips on the main camera.
    /// </summary>
    /// <param name="clip">This is the video to be played.</param>
    /// <param name="func">Callback function to be executed at the end of the video.</param>
    /// <param name="time">Delay the playback time by the argument.</param>
    public void CallPlayVideo(VideoClip clip, Action func, float time) => StartCoroutine(WaitForPlayVideo(clip, func, time));

    // Calls the PlayVideo coroutine with a delay as long as the time received as an argument.
    IEnumerator WaitForPlayVideo(VideoClip clip, Action func, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        StartCoroutine(PlayVideo(clip, func));
    }


    IEnumerator PlayVideo(VideoClip clip, Action func)
    {
        if (videoPlayer.targetCamera != Camera.main) videoPlayer.targetCamera = Camera.main;
        videoPlayer.clip = clip;

        videoPlayer.loopPointReached += (VideoPlayer vp) =>
        {
            TimeControl.Play();
            videoPlayer.Stop();
            func();
        };
        videoFunc = func;
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared == true);
        GameObject fadeInOutPanel = GameObject.Find("FadeInOutPanel");
        //Del & Add Aim UI 
        if (fadeInOutPanel != null)
            fadeInOutPanel.SetActive(false);
        TimeControl.Pause();
        videoPlayer.Play();
    }

    Action videoFunc = null;
    public void SkipVideo()
    {
        if (!GameManager.Instance.isFirstPlay && Input.GetKeyDown(KeyCode.Escape) && videoPlayer.isPlaying)
        {
            Debug.Log("SkipVideo");
            StopCoroutine("PlayVideo");
            TimeControl.Play();
            videoPlayer.Stop();
            videoFunc();
            videoFunc = null;
        }
    }

    public VideoClip GetClip(VideoCategory category, string name) => clips.GetValue(category.ToString() + "_" + name);


}
